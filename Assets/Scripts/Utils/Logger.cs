using System;
using DataObjects;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Utils
{
    /**
     * Contains methods for debugging 
     */
    public static class Logger
    {
        /**
         * Prints current time with text parameter into console
         */
        public static void LogTime(string text)
        {
            Debug.Log(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff ") + text);
        }

        /**
         * Prints message from parameter into console
         */
        public static void Print(string text)
        {
            Debug.Log(text);
        }

        /**
         * Visualize terrain borders
         */
        public static void CheckTerrainBorders(LatLngObject mapMiddlePoint)
        {
            var left = new LatLngObject(mapMiddlePoint.Latitude, mapMiddlePoint.Longitude);
            left.Latitude += 0.0116f;
            left.Longitude += 0.004f;
            var sphereL = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphereL.transform.position = Converter.ConvertLatLngToXyz(left);

            var right = new LatLngObject(mapMiddlePoint.Latitude, mapMiddlePoint.Longitude);
            right.Latitude -= 0.0122f;
            right.Longitude -= 0.004f;
            var sphereR = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphereR.transform.position = Converter.ConvertLatLngToXyz(right);

            var top = new LatLngObject(mapMiddlePoint.Latitude, mapMiddlePoint.Longitude);
            top.Latitude -= 0.0035f;
            top.Longitude += 0.0134f;
            var sphereT = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphereT.transform.position = Converter.ConvertLatLngToXyz(top);

            var bottom = new LatLngObject(mapMiddlePoint.Latitude, mapMiddlePoint.Longitude);
            bottom.Latitude += 0.0033f;
            bottom.Longitude -= 0.0145f;
            var sphereB = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphereB.transform.position = Converter.ConvertLatLngToXyz(bottom);
        }
    }
}