using System;
using System.Collections.Generic;
using DataObjects;
using UnityEngine;

namespace Utils
{
    /**
     * Parameters for map dimensions
     */
    public static class TerrainUtils
    {
        //x
        public const byte MapWidth = 33;

        //z
        public const byte MapHeight = 33;

        //+y
        public const byte MapTopValue = 2;

        //-y
        public const byte MapDepth = 2;


        /**
         * Check if object position is outside the map
         */
        public static bool IsObjectOutsideMap(Vector3 objectPos, Vector3 middleMapPoint)
        {
            if (objectPos.x > middleMapPoint.x + MapWidth / 2
                || objectPos.x < middleMapPoint.x - MapWidth / 2
                || objectPos.z > middleMapPoint.z + MapHeight / 2
                || objectPos.z < middleMapPoint.z - MapHeight / 2)
                return true;
            return false;
        }

        /**
         * Create LatLng terrain grid for Elevation API
         */
        public static List<LatLngObject> GetLatLngGrid(LatLngObject middlePoint)
        {
            var latLngGrid = new List<LatLngObject>();

            var leftBorder = new LatLngObject(middlePoint.Latitude, middlePoint.Longitude);
            leftBorder.Latitude += 0.0116f;
            leftBorder.Longitude += 0.004f;
            var rightBorder = new LatLngObject(middlePoint.Latitude, middlePoint.Longitude);
            rightBorder.Latitude -= 0.0122f;
            rightBorder.Longitude -= 0.004f;
            var topBorder = new LatLngObject(middlePoint.Latitude, middlePoint.Longitude);
            topBorder.Latitude -= 0.0035f;
            topBorder.Longitude += 0.0134f;
            var bottomBorder = new LatLngObject(middlePoint.Latitude, middlePoint.Longitude);
            bottomBorder.Latitude += 0.0033f;
            bottomBorder.Longitude -= 0.0145f;

            //lat
            var mapLengthLat = Math.Abs(leftBorder.Latitude - rightBorder.Latitude);
            var mapPartLat = mapLengthLat / MapWidth;
            var mapStartLat = leftBorder.Latitude < rightBorder.Latitude ? leftBorder.Latitude : rightBorder.Latitude;
            mapStartLat -= mapPartLat;

            //lon
            var mapLengthLon = Math.Abs(topBorder.Longitude - bottomBorder.Longitude);
            var mapPartLon = mapLengthLon / MapHeight;
            var mapStartLon = topBorder.Longitude < bottomBorder.Longitude ? topBorder.Longitude : bottomBorder.Longitude;
            var defaultLon = mapPartLon * 33 + mapStartLon;

            var lats = new float[MapHeight];
            for (var i = 0; i < MapHeight; i++)
            {
                lats[i] = mapStartLat += mapPartLat;
            }

            for (var i = 0; i < MapWidth; i++)
            {
                mapStartLon = defaultLon;

                for (var j = 0; j < MapHeight; j++)
                {
                    mapStartLon -= mapPartLon;
                    latLngGrid.Add(new LatLngObject(lats[32 - i], mapStartLon));
                }
            }

            return latLngGrid;
        }
    }
}