using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects;
using UnityEngine;
using Utils;

namespace _3D
{
    /**
     * 
     */
    public class RoadRender : MonoBehaviour
    {
        //SINGLETON
        private static RoadRender roadRender;


        private List<RoadObject> Roads;


        /**
         * Singleton instance getter for road render
         */
        public static RoadRender Get()
        {
            if (roadRender == null)
                roadRender = (new GameObject("RoadRender")).AddComponent<RoadRender>();
            return roadRender;
        }

        /**
         * Render roads
         */
        public void Render(LatLngObject mapMiddleMapPoint, Transform cameraTransform, List<RoadObject> roads)
        {
            if (roads.Count == 0) return;

            var middleMapPoint = Converter.ConvertLatLngToXyz(mapMiddleMapPoint);

            //save into global variable
            Roads = roads;
            var terrain = TerrainRender.Get().Terrain;
            //render roads
            for (var i = 0; i < roads.Count; i++)
            {
                var ySize = 1;

                var roadPointInXyz = new Vector3[roads[i].LatLngCoordinates.Count];
                if (roadPointInXyz.Length == 0)
                    continue;

                //convert road point from LatLng to Xyz coordination's; Y coordinate values set as terrain height
                for (var j = 0; j < roadPointInXyz.Length; j++)
                {
                    roadPointInXyz[j] = Converter.ConvertLatLngToXyz(roads[i].LatLngCoordinates[j]);
                    //pricteni hodnoty navic proti prolinani s terenem
                    roadPointInXyz[j].y = terrain.SampleHeight(roadPointInXyz[j]) + 0.03f;
                }

                //road starts outside terrain => reverse points
                if (TerrainUtils.IsObjectOutsideMap(roadPointInXyz[0], middleMapPoint))
                    Array.Reverse(roadPointInXyz);

                //points of road
                for (var j = 0; j < roadPointInXyz.Length; j++)
                {
                    var roadPoint = roadPointInXyz[j];
                    //pokud je mezi dvěma body cesty rozdíl větší než 1
                    if (j == 0 || Math.Abs(roadPoint.x - roadPointInXyz[j - 1].x) >= 1 || Math.Abs(roadPoint.z - roadPointInXyz[j - 1].z) >= 1)
                        continue;

                    //snižovat souřadnici Y cesty, pokud je za hranou terénu (efekt padání cesty)
                    if (TerrainUtils.IsObjectOutsideMap(roadPoint, middleMapPoint))
                    {
                        roadPointInXyz[j].y -= ySize;
                        ySize++;
                    }
                }

                //vytvoreni cesty(nastaveni barvy, pozic, poctu pozic...)
                CreateRoadObject(roads, roadPointInXyz, i);

                //set cars for road type
                var carRoads = new[] {RoadUtils.Secondary};
                var vehicleTypes = new[] {RoadUtils.VehicleTram};
                if (Main.Settings.RoadObjectsToggle && carRoads.Contains(roads[i].RoadType) || vehicleTypes.Contains(roads[i].VehicleType))
                {
                    roads[i].XyzCoordinates = roadPointInXyz;
                    RoadObjectRender.Get().RenderObjectsOnRoad(roads[i], middleMapPoint);
                }

                UiUtils.SetDefaultCameraPos(middleMapPoint, cameraTransform);
            }
        }

        /**
         * Create GameObject for road
         */
        private void CreateRoadObject(List<RoadObject> roads, Vector3[] roadPointInXyz, int i)
        {
            var roadType = roads[i].RoadType;
            var lineGameObject = new GameObject("road_" + i);
            lineGameObject.transform.Rotate(Vector3.right, 90);
            lineGameObject.transform.position = roadPointInXyz[0];
            lineGameObject.AddComponent<LineRenderer>();
            lineGameObject.AddComponent<PolygonCollider2D>();
            var lineRenderer = lineGameObject.GetComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = ColorUtils.DefaultRoute;
            lineRenderer.endColor = ColorUtils.DefaultRoute;
            lineRenderer.alignment = LineAlignment.TransformZ;
            lineRenderer.startWidth = RoadUtils.GetRoadWidth(roadType);
            lineRenderer.endWidth = RoadUtils.GetRoadWidth(roadType);
            lineRenderer.positionCount = roadPointInXyz.Length;
            lineRenderer.SetPositions(roadPointInXyz);
        }

        /**
         * Change road colors
         */
        public void ChangeRoadColors(bool defaultColor)
        {
            for (var i = 0; i < Roads.Count; i++)
            {
                var roadGameObject = GameObject.Find("road_" + i);
                var lineRenderer = roadGameObject.GetComponent<LineRenderer>();
                Color roadColor;
                if (!defaultColor)
                {
                    roadColor = RoadUtils.GetRoadColor(Roads[i].RoadType);
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                }
                else
                {
                    roadColor = ColorUtils.DefaultRoute;
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                }

                lineRenderer.startColor = roadColor;
                lineRenderer.endColor = roadColor;
            }
        }
    }
}