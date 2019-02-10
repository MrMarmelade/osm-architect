using System.Collections.Generic;
using DataObjects;
using UnityEngine;


namespace Utils
{
    /**
     * Methods for converting between different coordinate types
     */
    public static class Converter
    {
        /**
         * 
         */
        public static List<ThreeDimObject> GetBuildingsInXyz(List<BuildingObject> buildings)
        {
            var threeDimObjects = new List<ThreeDimObject>();
            foreach (var building in buildings)
            {
                var xyzCoordinates = new List<Vector3>();
                foreach (var latLng in building.LatLngCoordinates)
                {
                    xyzCoordinates.Add(ConvertLatLngToXyz(latLng));
                }

                var threeDimObject = new ThreeDimObject(xyzCoordinates, building.BuildingHeight, building.AddressObject);
                threeDimObjects.Add(threeDimObject);
            }

            return threeDimObjects;
        }

        /**
         * Converts LatLng coordinates into XYZ format
         * @see https://answers.unity.com/questions/923182/lat-long-to-unity-world-coordinates.html
         */
        public static Vector3 ConvertLatLngToXyz(LatLngObject latLng)
        {
            if (latLng == null) return Vector3.zero;

            const int scaleUp = 100000;
            Vector3 xyzPosition = Quaternion.AngleAxis(latLng.Longitude, -Vector3.up)
                                  * Quaternion.AngleAxis(latLng.Latitude, -Vector3.right)
                                  * new Vector3(0, 0, 1);
            var scaledXyzPosition = new Vector3(xyzPosition.x * scaleUp, 0f, xyzPosition.z * scaleUp);
            return scaledXyzPosition;
        }
    }
}