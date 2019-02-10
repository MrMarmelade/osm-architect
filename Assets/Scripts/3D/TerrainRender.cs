using System.Collections.Generic;
using System.Linq;
using DataObjects;
using Parser;
using UnityEngine;
using Utils;

namespace _3D
{
    /**
     * 
     */
    public class TerrainRender : MonoBehaviour
    {
        //SINGLETON
        private static TerrainRender terrainRender;


        public Terrain Terrain;


        /**
         * Get singleton object for terrain render
         */
        public static TerrainRender Get()
        {
            if (terrainRender == null)
                terrainRender = (new GameObject("TerrainRender")).AddComponent<TerrainRender>();
            return terrainRender;
        }

        /**
         * Generate map terrain
         */
        public void GenerateTerrain(LatLngObject mapMiddleMapPoint, Transform cameraTransform)
        {
            Vector3 mapMiddlePoint;
            //todo refactorovat tuhle podminku obecne u vsech middle pointu
            if (mapMiddleMapPoint.Equals(new LatLngObject()))
            {
                mapMiddlePoint = Vector3.zero;
                UiUtils.SetDefaultCameraPos(mapMiddlePoint, cameraTransform);
            }
            else
            {
                mapMiddlePoint = Converter.ConvertLatLngToXyz(mapMiddleMapPoint);
            }

            //create underground
            CreateUnderground(mapMiddlePoint);

            //create terrain
            var terrainGameObject = new GameObject("TerrainObj");
            terrainGameObject.AddComponent<Terrain>();
            terrainGameObject.AddComponent<TerrainCollider>();

            Terrain = terrainGameObject.GetComponent<Terrain>();
            var terrainData = new TerrainData
            {
                heightmapResolution = TerrainUtils.MapWidth,
                size = new Vector3(TerrainUtils.MapWidth, TerrainUtils.MapTopValue, TerrainUtils.MapHeight)
            };

            //load default or elevationAPI terrain
            float[,] heightsData = Main.Settings.TerrainToggle ? ConvertHeights() : GenerateHeightDemo();

            terrainData.SetHeights(0, 0, heightsData);
            Terrain.terrainData = terrainData;
            //odkomentovat jen, pokud je vyska terenu vetsi nez 0
//            terrainGameObject.GetComponent<TerrainCollider>().terrainData = terrainData;
            Terrain.materialType = Terrain.MaterialType.BuiltInLegacyDiffuse;
            AddTexture(Terrain.terrainData, Resources.Load<Texture2D>("Textures/grass_texture"));
            mapMiddlePoint.x -= TerrainUtils.MapWidth / 2;
            mapMiddlePoint.y = -0.01f;
            mapMiddlePoint.z -= TerrainUtils.MapHeight / 2;
            terrainGameObject.transform.position = mapMiddlePoint;
        }

        /**
         * Create object for underground, located under terrain
         */
        private void CreateUnderground(Vector3 mapMiddlePoint)
        {
            var undergroundTerrain = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var undergroundCenter = mapMiddlePoint;
            //hodnoty .5 a .5 slouzi pro zarovnani terenu s podlozim, konretne pro velikost 33x33
            undergroundCenter.x = undergroundCenter.x + 0.5f;
            undergroundCenter.y = -1.014f;
            undergroundCenter.z = undergroundCenter.z + 0.5f;
            undergroundTerrain.transform.position = undergroundCenter;
            //velikost objektu pro podlozi terrainu
            undergroundTerrain.transform.localScale = new Vector3(
                TerrainUtils.MapWidth,
                TerrainUtils.MapDepth,
                TerrainUtils.MapHeight
            );
            var undergroundMaterial = undergroundTerrain.GetComponent<Renderer>().material;
            undergroundMaterial.mainTexture = Resources.Load<Texture2D>("Textures/rock");
            undergroundMaterial.mainTextureScale = new Vector3(1f, 1f, 1f);
        }

        /**
         * Transform ElevationAPI data into terrain heights
         */
        private float[,] ConvertHeights()
        {
            var heights = AltitudeLoader.Get().AltitudeData;
            var convertedHeights = new float[TerrainUtils.MapWidth, TerrainUtils.MapHeight];
            var maxValue = heights.Max(x => x.Elevation);
            var minValue = heights.Min(y => y.Elevation);
            var heightsIndex = 0;
            for (var i = 0; i < TerrainUtils.MapWidth; i++)
            {
                for (var j = 0; j < TerrainUtils.MapHeight; j++)
                {
                    //creating of terrain edges
                    if (i == 0 || j == 0 || i == TerrainUtils.MapWidth - 1 || j == TerrainUtils.MapWidth - 1)
                    {
                        convertedHeights[i, j] = 0;
                    }
                    else
                    {
                        convertedHeights[i, j] = (heights[heightsIndex].Elevation - minValue) / (maxValue - minValue);
                    }

                    ++heightsIndex;
                }
            }

            return convertedHeights;
        }

        /**
         * Demo for generating values for terrain heights
         */
        private float[,] GenerateHeightDemo()
        {
            var heights = new float[TerrainUtils.MapWidth, TerrainUtils.MapHeight];
            for (var i = 0; i < TerrainUtils.MapWidth; i++)
            {
                for (var j = 0; j < TerrainUtils.MapHeight; j++)
                {
                    if (i == 0 || j == 0 || i == TerrainUtils.MapWidth - 1 || j == TerrainUtils.MapHeight - 1)
                    {
                        heights[i, j] = 0f;
                    }
                    else
                    {
                        heights[i, j] = 0.5f;
                    }
                }
            }

            return heights;
        }

        /**
         * Apply texture on terrain
         */
        private void AddTexture(TerrainData terrainData, Texture2D tex)
        {
            List<SplatPrototype> protos = terrainData.splatPrototypes.ToList();
            SplatPrototype newSplatProto = new SplatPrototype
            {
                texture = tex,
                tileOffset = Vector2.zero,
                tileSize = Vector2.one
            };
            newSplatProto.texture.Apply(true);
            protos.Add(newSplatProto);
            terrainData.splatPrototypes = protos.ToArray();
        }

        /**
         * Find middle point from list of buildings coordinates
         */
        public static LatLngObject GetMiddlePoint(List<BuildingObject> buildings)
        {
            if (buildings.Count == 0) return null;

            var lat = new List<float>();
            var lon = new List<float>();
            foreach (var building in buildings)
            {
                foreach (var latLngObject in building.LatLngCoordinates)
                {
                    lat.Add(latLngObject.Latitude);
                    lon.Add(latLngObject.Longitude);
                }
            }

            var finalLat = (lat.Min() + lat.Max()) / 2;
            var finalLon = (lon.Min() + lon.Max()) / 2;
            return new LatLngObject(finalLat, finalLon);
        }

        /**
         * Find middle point from list of roads coordinates
         */
        public static LatLngObject GetMiddlePoint(List<RoadObject> roads)
        {
            var lat = new List<float>();
            var lon = new List<float>();
            foreach (var road in roads)
            {
                foreach (var latLngObject in road.LatLngCoordinates)
                {
                    lat.Add(latLngObject.Latitude);
                    lon.Add(latLngObject.Longitude);
                }
            }

            var finalLat = (lat.Min() + lat.Max()) / 2;
            var finalLon = (lon.Min() + lon.Max()) / 2;
            return new LatLngObject(finalLat, finalLon);
        }
    }
}