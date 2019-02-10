using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Utils;

namespace DataObjects.FileStructure
{
    /**
     * 
     */
    [XmlRoot("Map")]
    public class Map
    {
        //full path (for example: Application.dataPath, "./Maps/" + "/mapvinohrady.osm")
        public string MapName { get; set; }

        //unix timestamp
        public long Created { get; set; }

        //unix timestamp
        public long LastSave { get; set; }

        [XmlArray("CustomObjectsAdded"), XmlArrayItem("CustomObject")]
        public CustomObject[] AddedCustomObjects { get; set; }

        [XmlArray("MapObjectsRemoved"), XmlArrayItem("MapObject")]
        public MapObject[] RemovedMapObjects { get; set; }


        /**
         * Save map data into XML file
         */
        public void Save()
        {
            Created = (int) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            LastSave = (int) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var LastSaveName = DateTime.Now.ToString("ddMMyy_HHmmss");

            //add custom map objects
            AddedCustomObjects = Main.AddedObjects.ToArray();

            //add removed map objects
            var removedMapObjectsList = new List<MapObject>();
            foreach (var removedObject in Main.RemovedObjects)
            {
                var mapObject = new MapObject {Position = removedObject.XyzCoordinates};
                removedMapObjectsList.Add(mapObject);
            }

            RemovedMapObjects = removedMapObjectsList.ToArray();

            //write into xml file
            var serializer = new XmlSerializer(typeof(Map));
            using (var stream = new FileStream(Path.Combine(Application.dataPath, FileUtils.SavesPath + "save_" + LastSaveName + ".xml"),
                FileMode.Create))
            {
                serializer.Serialize(stream, this);
            }
        }

        /**
         * Load map data from XML file
         *
         * hint how to load rotation: load rotation as customObject.transform.Rotate(new Vector3(0, hodnota rotace v xml, 0));
         */
        public static Map Load(string path)
        {
            var serializer = new XmlSerializer(typeof(Map));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as Map;
            }
        }
    }
}