using System.Collections.Generic;
using System.IO;
using System.Xml;
using DataObjects;
using UnityEngine;
using Utils;

namespace Parser
{
    /**
     * 
     */
    public class TreeLoader : MonoBehaviour
    {
        //SINGLETON
        private static TreeLoader treeLoader;


        /**
         * Singleton for tree loader
         */
        public static TreeLoader Get()
        {
            if (treeLoader == null)
                treeLoader = (new GameObject("TreeLoader")).AddComponent<TreeLoader>();
            return treeLoader;
        }

        /**
         * 
         */
        public List<TreeObject> LoadFile()
        {
            if (!FileUtils.IsFileExist(FileUtils.TreePath, "/trees_data.xml"))
                return new List<TreeObject>();

            //load file with tree coordinates
            var xmlFileTrees = new XmlDocument();
            xmlFileTrees.Load(Path.Combine(Application.dataPath, FileUtils.TreePath + "/trees_data.xml"));
            var treeTags = xmlFileTrees.GetElementsByTagName("T");
            return ParseTrees(treeTags);
        }

        /**
         * Parse trees from XML file
         */
        private List<TreeObject> ParseTrees(XmlNodeList treeTags)
        {
            var treeObjects = new List<TreeObject>();
            //<T> tags
            foreach (XmlNode treeTag in treeTags)
            {
                var treeObject = new TreeObject();
                var latLngCoordinate = new LatLngObject();
                //tags in <T> tag
                for (var i = 0; i < treeTag.ChildNodes.Count; i++)
                {
                    //lat or lng tag
                    var coordinateTag = treeTag.ChildNodes[i];
                    //coordinate
                    var coordinate = float.Parse(coordinateTag.InnerText);
                    if (i == 0)
                        latLngCoordinate.Latitude = coordinate;
                    else
                        latLngCoordinate.Longitude = coordinate;
                }

                treeObject.LatLngCoordinate = latLngCoordinate;
                treeObjects.Add(treeObject);
            }

            return treeObjects;
        }
    }
}