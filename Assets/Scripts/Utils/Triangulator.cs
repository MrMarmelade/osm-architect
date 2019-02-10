using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    /**
     * 
     */
    public static class Triangulator
    {
        private static List<Vector3> points = new List<Vector3>();
        private static List<Vector3> outputPoints = new List<Vector3>();
        private static List<int> outputTriangles = new List<int>();

        private static float minX;
        private static float minZ;
        private static float maxX;
        private static float maxZ;


        /**
         * Generate triangle indexes for building meshes
         */
        public static int[] GenerateTriangleNewVersion(List<Vector3> vertices)
        {
            CleanLists();
            //copy vertices
            points.AddRange(vertices);

            while (points.Count >= 3)
            {
                GetMaxPoints();
                AddTriangle();
            }

            //find index of vertices in original list
            foreach (var point in outputPoints)
            {
                var indexPoint = vertices.IndexOf(point);
                outputTriangles.Add(indexPoint);
            }

            return outputTriangles.ToArray();
        }

        /**
         * Create triangle
         */
        private static void AddTriangle()
        {
            var a = Vector3.zero;
            var b = Vector3.zero;
            var c = Vector3.zero;
            for (var i = 0; i < points.Count; i++)
            {
                //vertex a
                a = points[i];

                //vertex b
                if (i + 1 == points.Count)
                    b = points[0];
                else
                    b = points[i + 1];

                //vertex c
                if (i + 2 == points.Count)
                    c = points[0];
                else if (i + 2 == points.Count + 1)
                    c = points[1];
                else
                    c = points[i + 2];

                //if vertex b has a maximal value
                if (b.x == minX || b.x == maxX || b.z == minZ || b.z == maxZ)
                    break;
            }

            //select points for triangle
            outputPoints.Add(a);
            outputPoints.Add(b);
            outputPoints.Add(c);
            //remove point B
            points.Remove(b);
        }

        /**
         * Clear static lists
         */
        private static void CleanLists()
        {
            points.Clear();
            outputPoints.Clear();
            outputTriangles.Clear();
        }

        /**
         * Get maximal X and Z values
         */
        private static void GetMaxPoints()
        {
            minX = points.Any() ? points.Min(point => point.x) : 0;
            maxX = points.Any() ? points.Max(point => point.x) : 0;
            minZ = points.Any() ? points.Min(point => point.z) : 0;
            maxZ = points.Any() ? points.Max(point => point.z) : 0;
        }
    }
}