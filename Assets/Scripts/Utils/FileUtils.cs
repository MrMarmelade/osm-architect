using System;
using System.IO;
using UnityEngine;

namespace Utils
{
    /**
     * Methods for checking status and creating of folders for user
     */
    public static class FileUtils
    {
        public const byte StorageLocal = 0;
        public const byte StorageNetwork = 1;

        public const string SavesPath = "./Saves/";
        public const string MapsPath = "./Maps/";
        public const string ObjectsPath = "./3DObjects/";
        public const string TreePath = "./Trees/";

        public const string MapFileExtension = ".osm";


        /**
         * Check if custom object exists
         */
        public static bool IsCustomObjectExist(string customObjectName)
        {
            return File.Exists(Path.Combine(Application.dataPath, ObjectsPath + customObjectName));
        }


        /**
         * Check if map file exists (for example: brno.osm as @param mapName)
         */
        public static bool IsMapNameExist(string mapName)
        {
            return File.Exists(Path.Combine(Application.dataPath, MapsPath + mapName));
        }

        /**
         * Get full map path
         */
        public static string GetFullMapPath(string mapFile)
        {
            var mapFullPath = Path.Combine(Application.dataPath, MapsPath + mapFile);
            return mapFullPath;
        }

        /**
         * Get full custom object path
         */
        public static string GetFullCustomObjectPath()
        {
            var customObjectFullPath = Path.Combine(Application.dataPath, ObjectsPath);
            return customObjectFullPath;
        }

        /**
         * Check saves folder and create it
         */
        public static void CheckSaveFolder()
        {
            var savesFolderPath = Path.Combine(Application.dataPath, SavesPath);
            if (!Directory.Exists(savesFolderPath))
                Directory.CreateDirectory(savesFolderPath);
        }

        /**
         * Check maps folder and create it
         */
        public static void CheckMapsFolder()
        {
            var mapFolderPath = Path.Combine(Application.dataPath, MapsPath);
            if (!Directory.Exists(mapFolderPath))
                Directory.CreateDirectory(mapFolderPath);
        }

        /**
         * Check custom models folder and create it
         */
        public static void CheckCustomObjectsFolder()
        {
            var customObjFolderPath = Path.Combine(Application.dataPath, ObjectsPath);
            if (!Directory.Exists(customObjFolderPath))
                Directory.CreateDirectory(customObjFolderPath);
        }

        /**
         * Check tree folder and create it
         */
        public static void CheckTreeFolder()
        {
            var treeFolderPath = Path.Combine(Application.dataPath, TreePath);
            if (!Directory.Exists(treeFolderPath))
                Directory.CreateDirectory(treeFolderPath);
        }

        /**
         * Check if file exists
         */
        public static bool IsFileExist(string folder, string fileName)
        {
            return File.Exists(Path.Combine(Application.dataPath, folder + fileName));
        }

        /**
         * Check API key validity
         */
        public static bool CheckKey(String apiKey)
        {
            if (apiKey.StartsWith("AIza"))
                return true;
            return false;
        }

        /**
         * Create all necessary folders for user
         */
        public static void CreateStartFolders()
        {
            CheckSaveFolder();
            CheckMapsFolder();
            CheckCustomObjectsFolder();
            CheckTreeFolder();
        }
    }
}