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
    [XmlRoot("Key")]
    public class Key
    {
        //
        public string ApiKey { get; set; }


        /**
         * Save API key into XML file
         */
        public void Save()
        {
            var path = Path.Combine(Application.dataPath, SettingsSceneSet.FilePathKey);
            //write into xml file
            var serializer = new XmlSerializer(typeof(Key));
            using (var stream = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(stream, this);
            }
        }

        /**
         * Load API key data from XML file
         */
        public static Key Load(string path)
        {
            var serializer = new XmlSerializer(typeof(Key));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as Key;
            }
        }
    }
}