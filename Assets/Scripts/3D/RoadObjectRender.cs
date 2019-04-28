using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects;
using UnityEngine;
using Utils;
using Random = System.Random;

namespace _3D
{
    /**
     * Metody pro vytvoření vozidel a jejich následný pohyb po cestě
     */
    public class RoadObjectRender : MonoBehaviour
    {
        //SINGLETON
        private static RoadObjectRender roadObjectRender;

        private GameObject BlueCarPrefab;
        private GameObject GreenCarPrefab;
        private GameObject BlueTruckPrefab;
        private GameObject TramPrefab;

        private Random Random;
        private List<RoadObject> RoadObjects;
        private int NameIndex;
        private List<GameObject> Cars;
        private List<float> Path;
        private Vector3 MiddleMapPoint;


        /**
         * Singleton for road object render
         */
        public static RoadObjectRender Get()
        {
            if (roadObjectRender == null)
                roadObjectRender = (new GameObject("RoadObjectRender")).AddComponent<RoadObjectRender>();
            return roadObjectRender;
        }


        //This method is called when RoadObjectRender is added into Main using AddComponent<RoadObjectRender>()
        private void Awake()
        {
            RoadObjects = new List<RoadObject>();
            Cars = new List<GameObject>();
            Path = new List<float>();
            Random = new Random();
            //load car prefabs
            BlueCarPrefab = Resources.Load("3DObjects/Cartoon Vehicles/Prefabs/BLUE CAR", typeof(GameObject)) as GameObject;
            GreenCarPrefab = Resources.Load("3DObjects/Cartoon Vehicles/Prefabs/CAR GREEN", typeof(GameObject)) as GameObject;
            BlueTruckPrefab = Resources.Load("3DObjects/Cartoon Vehicles/Prefabs/TRUCK_BLUE", typeof(GameObject)) as GameObject;
            TramPrefab = Resources.Load("3DObjects/Trams/tram1", typeof(GameObject)) as GameObject;
        }

        /**
         * 
         */
        private void Update()
        {
            MoveWithRoadObjects();
        }

        /**
         * Pro rozpohybování vozidla po cestě
         */
        private void MoveWithRoadObjects()
        {
            for (var j = 0; j < RoadObjects.Count; j++)
            {
                var roadObject = RoadObjects[j];
                if (roadObject.CurrentIndexPosition + 1 == roadObject.XyzCoordinates.Count())
                    roadObject.CurrentIndexPosition = 0;

                var startPos = roadObject.XyzCoordinates[roadObject.CurrentIndexPosition];
                var endPos = roadObject.XyzCoordinates[roadObject.CurrentIndexPosition + 1];

                if (Cars[j].transform.position == endPos)
                    roadObject.CurrentIndexPosition = roadObject.CurrentIndexPosition + 1;

                Path[j] += 0.02f;
                if (Path[j] >= 1)
                {
                    Path[j] = 0;
                    continue;
                }

                //rotate vehicle
                //get direction vector
                var targetDirection = endPos - startPos;
                var newDir = Vector3.RotateTowards(transform.forward, targetDirection, 60f, 0.0f);
                //vehicle rotation
                Cars[j].transform.rotation = Quaternion.LookRotation(newDir);
                //move with vehicle between two points of road
                Cars[j].transform.position = Vector3.Lerp(startPos, endPos, Path[j]);


                //if vehicle is outside the map
                if (!(Cars[j].transform.position.x > MiddleMapPoint.x - ((TerrainUtils.MapWidth / 2) - 0.05) &&
                      Cars[j].transform.position.x < MiddleMapPoint.x + ((TerrainUtils.MapWidth / 2) - 0.05) &&
                      Cars[j].transform.position.z > MiddleMapPoint.z - ((TerrainUtils.MapWidth / 2) - 0.05) &&
                      Cars[j].transform.position.z < MiddleMapPoint.z + ((TerrainUtils.MapWidth / 2) - 0.05)))
                {
                    Path[j] = 0;
                }
            }
        }

        /**
         * Vytvoření 3D objektu vozidla na dané cestě
         */
        public void RenderObjectsOnRoad(RoadObject roadObject, Vector3 middleMapPoint)
        {
            MiddleMapPoint = middleMapPoint;
            //do not add road created only from one point
            if (roadObject.XyzCoordinates.Count() < 2) return;

            //pokud je objekt tramvaj a pokud se objekt pohybuje jen na draze mensi nez jedna(X a zaroven Z), tak ho nepridavat
            var startCoordinate = roadObject.XyzCoordinates[0];
            var lastCoordinate = roadObject.XyzCoordinates[roadObject.XyzCoordinates.Length - 1];
            if (roadObject.VehicleType == RoadUtils.VehicleTram && Math.Abs(startCoordinate.x - lastCoordinate.x) < 1 &&
                Math.Abs(startCoordinate.z - lastCoordinate.z) < 1) return;

            //velikost objektu vozidel
            var vehicleScale = 0.018f;

            //nastavení vozidla do prvního úseku
            //pro pohyb mezi všemi úseky až do konce cesty
            roadObject.CurrentIndexPosition = 0;
            RoadObjects.Add(roadObject);

            //nastavení daného vozidla na začátek jednoho z úseků cesty (vozidlo se v jednom úseku pohybuje v rozmezí 0-1)
            //pro pohyb v jednom konkrétním úseku cesty
            Path.Add(0f);

            var carTypeRandom = 0;
            //random number for selecting vehicle type
            if (roadObject.VehicleType == RoadUtils.VehicleCar)
            {
                //0 -- 2 (3 type of cars)
                carTypeRandom = Random.Next(0, 3);
            }
            //tram (1 type of tram)
            else if (roadObject.VehicleType == RoadUtils.VehicleTram)
            {
                carTypeRandom = 3;
            }

            GameObject car;
            switch (carTypeRandom)
            {
                //cars
                case 0:
                    car = Instantiate(BlueCarPrefab);
                    break;
                case 1:
                    car = Instantiate(GreenCarPrefab);
                    break;
                case 2:
                    car = Instantiate(BlueTruckPrefab);
                    break;
                //trams
                case 3:
                    car = Instantiate(TramPrefab);
                    break;
                //default
                default:
                    car = Instantiate(BlueTruckPrefab);
                    break;
            }

            car.transform.localScale = new Vector3(vehicleScale, vehicleScale, vehicleScale);
            car.name = "car_" + (++NameIndex);
            Cars.Add(car);
        }
    }
}