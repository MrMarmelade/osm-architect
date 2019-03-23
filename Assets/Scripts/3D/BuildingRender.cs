using System.Collections.Generic;
using System.Linq;
using Controls;
using DataObjects;
using UnityEditor;
using UnityEngine;
using Utils;


namespace _3D
{
    /**
     * Class for 3D object rendering
     */
    public class BuildingRender : MonoBehaviour
    {
        //SINGLETON
        private static BuildingRender buildingRender;

        private Shader BuildingShader;

        public List<GameObject> BatchedBuildings = new List<GameObject>();
        public List<ThreeDimObject> Buildings = new List<ThreeDimObject>();
        private float TerrainHeight;
        private const float BuildingFloorHeight = 0.1f;


        /**
         * Singleton for building render
         */
        public static BuildingRender Get()
        {
            if (buildingRender == null)
                buildingRender = (new GameObject("BuildingRender")).AddComponent<BuildingRender>();
            return buildingRender;
        }

        /**
         * Vytvoří objekty a ty následně vyrenderuje do hlavní scény
         */
        public void Render(Transform cameraTransform, List<BuildingObject> buildings)
        {
            //cache shader into variable
            BuildingShader = Shader.Find("Ciconia Studio/Double Sided/Standard/Diffuse Bump");
            //render custom objects from save
            AddCustomObjectsFromSave();
            //no building in the map
            if (buildings.Count == 0) return;

            const string buildingName = "building_";
            var nameIndex = 0;
            var savedIndex = nameIndex;

            var threeDimObjects = Converter.GetBuildingsInXyz(buildings);
            foreach (var threeDimObject in threeDimObjects)
            {
                //remove destroyed building from saved map
                if (IsObjectDestroyed(threeDimObject))
                    continue;

                var groundEdges = GetGroundEdges(threeDimObject);
                var roofEdges = GetRoofEdges(threeDimObject);
                //vytvoreni strechy a podstavy (prepnuto z (i = 0) na (i = 1) == jen strechy)
                for (var i = 1; i < 2; i++)
                {
                    var triangleType = 0;
                    //four vertices buildings
                    if (groundEdges.Count == 4)
                        triangleType = 2;

                    threeDimObject.Name = buildingName + nameIndex;
                    Buildings.Add(threeDimObject);
                    CreateGameObject(threeDimObject.Name, roofEdges.ToArray(), triangleType);
                    nameIndex++;
                }

                //vytvoreni sten
                for (var i = 0; i < groundEdges.Count; i++)
                {
                    //vrcholy plochy
                    var edges = new Vector3[4];
                    var iNext = i;
                    if (i + 1 == groundEdges.Count)
                        iNext = 0;
                    else
                        iNext++;

                    //spodní body stěny
                    edges[0] = groundEdges[i];
                    edges[1] = groundEdges[iNext];
                    //horní body stěny
                    edges[2] = roofEdges[iNext];
                    edges[3] = roofEdges[i];
                    CreateGameObject(buildingName + nameIndex, edges, 1);
                    nameIndex++;
                }

                //group parts of building together
                var parentObj = GameObject.Find(buildingName + savedIndex);
                //set collider and listener for controller click
                parentObj.AddComponent<MeshCollider>();
                parentObj.AddComponent<GameObjectClick>();
                for (var i = savedIndex; i < nameIndex; i++)
                {
                    GameObject.Find(buildingName + i).transform.parent = parentObj.transform;
                }

                savedIndex = nameIndex;
            }

            //optimize building objects
            if (BatchedBuildings.Count > 0)
                StaticBatchingUtility.Combine(BatchedBuildings.ToArray(), BatchedBuildings[0]);
        }

        /**
         * Pridani vlastnich objektu, ktere jsou uvedeny v savu mapy
         */
        private void AddCustomObjectsFromSave()
        {
            if (Main.Map.AddedCustomObjects == null || Main.Map.AddedCustomObjects.Length == 0)
                return;

            foreach (var addedCustomObject in Main.Map.AddedCustomObjects)
            {
                MenuController.Get().LoadCustomObjectIntoMap(
                    addedCustomObject.ObjectName,
                    FileUtils.StorageLocal,
                    addedCustomObject.Position,
                    addedCustomObject.Size,
                    addedCustomObject.Rotation
                );
            }
        }

        /**
         * Pokud je objekt v savu oznacen jako zniceny, tak jej nepridavat do mapy
         */
        private bool IsObjectDestroyed(ThreeDimObject threeDimObject)
        {
            if (Main.Map.RemovedMapObjects == null || Main.Map.RemovedMapObjects.Length == 0)
                return false;

            foreach (var removedMapObject in Main.Map.RemovedMapObjects)
            {
                if (removedMapObject.Position.All(threeDimObject.XyzCoordinates.Contains) &&
                    removedMapObject.Position.Count == threeDimObject.XyzCoordinates.Count)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * Create new GameObject, part of a building
         *                  triangleType = 0 => generate triangles via Triangulator for roofs
         *                               = 1 => generate optimized triangles for walls = only two triangles is enough for 4 wall points
         *                               = 2 => generate triangles for 4 point roofs
         */
        private void CreateGameObject(string buildingName, Vector3[] edges, int triangleType)
        {
            var gameObjectArea = new GameObject(buildingName);
#if UNITY_EDITOR
            const StaticEditorFlags flags = StaticEditorFlags.BatchingStatic | StaticEditorFlags.LightmapStatic;
            GameObjectUtility.SetStaticEditorFlags(gameObjectArea, flags);
#endif
            var meshFilter = (MeshFilter) gameObjectArea.AddComponent(typeof(MeshFilter));
            var mesh = meshFilter.mesh;
            mesh.Clear();
            mesh.vertices = edges;
            //fill area with triangles
            int[] triangles = triangleType != 0 ? GenerateTrianglesFor4Points() : Triangulator.GenerateTriangleNewVersion(edges.ToList());

            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
#if UNITY_EDITOR
            MeshUtility.Optimize(mesh);
#endif
            ChangeBuildingSurfaceColor(gameObjectArea, ColorUtils.DefaultBuilding);
            BatchedBuildings.Add(gameObjectArea);
        }


        /**
         * Change color of building mesh
         */
        public void ChangeBuildingSurfaceColor(GameObject meshGameObject, Color color)
        {
            var texture2D = new Texture2D(1, 1);
            texture2D.SetPixel(0, 0, color);
            texture2D.Apply();

            MeshRenderer meshRenderer;
            if (meshGameObject.GetComponent(typeof(MeshRenderer)) == null)
                meshRenderer = meshGameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
            else
                meshRenderer = meshGameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;

            var material = meshRenderer.material;
            material.shader = BuildingShader;
            material.mainTexture = texture2D;
        }


        /**
         * Optimized triangles for walls of buildings or 4 point roofs (2 triangles is enough)
         */
        private int[] GenerateTrianglesFor4Points()
        {
            return new[]
            {
                0, 1, 2,
                2, 3, 0
            };
        }

        /**
         * Změna Y souřadnice stěn na hodnotu výšky terénu
         */
        private List<Vector3> GetGroundEdges(ThreeDimObject threeDimObject)
        {
            TerrainHeight = TerrainUtils.MapTopValue + 1f;
            var biggestTerrainHeight = 0f;
            var localCoordinates = threeDimObject.XyzCoordinates;

            foreach (var localCoordinate in localCoordinates)
            {
                var tempHeight = TerrainRender.Get().Terrain.SampleHeight(localCoordinate);
                //get biggest terrain height
                if (tempHeight > biggestTerrainHeight)
                    biggestTerrainHeight = tempHeight;

                //get lowest terrain height
                if (tempHeight < TerrainHeight)
                    TerrainHeight = tempHeight;
            }

            //no coordinates, set default value 
            if (TerrainHeight == TerrainUtils.MapTopValue + 1f)
                TerrainHeight = 0f;

            var groundEdges = new List<Vector3>();
            foreach (var localCoordinate in localCoordinates)
            {
                var v = localCoordinate;
                v.y = TerrainHeight;
                groundEdges.Add(v);
            }

            //save biggest terrain height for roof edges
            TerrainHeight = biggestTerrainHeight;

            return groundEdges;
        }

        /**
         * Zmena souřadnice y pro vykreslení střechy budovy, přičtení výšky terénu k výšce budovy
         */
        private List<Vector3> GetRoofEdges(ThreeDimObject threeDimObject)
        {
            var localCoordinates = threeDimObject.XyzCoordinates;
            var buildingHeight = threeDimObject.Height * BuildingFloorHeight;
            var roofEdges = new List<Vector3>();
            foreach (var localCoordinate in localCoordinates)
            {
                var v = localCoordinate;
                v.y = TerrainHeight + buildingHeight;
                roofEdges.Add(v);
            }

            return roofEdges;
        }
    }
}