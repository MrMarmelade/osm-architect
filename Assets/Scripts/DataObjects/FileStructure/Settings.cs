using System;
using System.IO;
using System.Xml.Serialization;
using UI;
using UnityEngine;

namespace DataObjects.FileStructure
{
    /**
     * 
     */
    [XmlRoot("Settings")]
    public class Settings
    {
        public long LastSave { get; set; }
        public int Language { get; set; }
        public bool TerrainToggle { get; set; }
        public bool RoadObjectsToggle { get; set; }
        public int NumberOfTrees { get; set; }
        public int StorageType { get; set; }

        public bool Tutorial { get; set; }


        /**
         * Save settings into XML file
         */
        public void Save()
        {
            var path = Path.Combine(Application.dataPath, SettingsSceneSet.FilePathSettings);

            LastSave = (int) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            //write into xml file
            var serializer = new XmlSerializer(typeof(Settings));
            using (var stream = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(stream, this);
            }
        }

        /**
         * Load settings from XML file
         */
        public static Settings Load(string path)
        {
            var serializer = new XmlSerializer(typeof(Settings));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as Settings;
            }
        }
    }
}