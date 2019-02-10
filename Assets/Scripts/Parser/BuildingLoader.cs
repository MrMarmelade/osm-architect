using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DataObjects;
using UnityEngine;
using Utils;
using _3D;

namespace Parser
{
    /**
     * Methods for parsing and processing data for building objects
     */
    public class BuildingLoader : MonoBehaviour
    {
        //SINGLETON
        private static BuildingLoader buildingLoader;


        private const float DefaultBuildingHeight = 0f;


        /**
         * Singleton for building loader
         */
        public static BuildingLoader Get()
        {
            if (buildingLoader == null)
                buildingLoader = (new GameObject("BuildingLoader")).AddComponent<BuildingLoader>();
            return buildingLoader;
        }

        /**
         * Parse data => call XML file parsing
         */
        public List<BuildingObject> LoadFile(XmlNodeList nodeTags, List<long> nodeTagIds, XmlNodeList wayTags)
        {
            //parse addresses
            var addresses = ParseAddresses(nodeTags);
            //parse buildings, assign addresses to buildings
            return ParseBuildings(wayTags, nodeTags, nodeTagIds, addresses);
        }

        /**
         * Parse building addresses from 'node' XML tags
         */
        private List<AddressObject> ParseAddresses(XmlNodeList nodeTags)
        {
            var addressObjects = new List<AddressObject>();
            foreach (XmlNode nodeTag in nodeTags)
            {
                if (nodeTag.ChildNodes.Count == 0)
                    continue;
                var address = new AddressObject();
                for (var i = 0; i < nodeTag.ChildNodes.Count; i++)
                {
                    //node tag
                    var tag = nodeTag.ChildNodes[i];
                    //key value in node tag
                    var tagKeyValue = tag.Attributes["k"].Value;
                    if (tagKeyValue.Equals("addr:street"))
                    {
                        address.Street = tag.Attributes["v"].Value;
                        continue;
                    }

                    if (tagKeyValue.Equals("addr:streetnumber"))
                    {
                        address.StreetNumber = tag.Attributes["v"].Value;
                    }
                }

                if (!address.Equals(new AddressObject()))
                {
                    var lat = float.Parse(nodeTag.Attributes["lat"].Value);
                    var lon = float.Parse(nodeTag.Attributes["lon"].Value);
                    address.Position = new LatLngObject(lat, lon);
                    addressObjects.Add(address);
                }
            }

            return addressObjects;
        }

        /**
         * Parse building data from 'way' XML tags
         */
        private List<BuildingObject> ParseBuildings(XmlNodeList wayTags, XmlNodeList nodeTags, List<long> nodeTagIds,
            List<AddressObject> addressObjects)
        {
            var isBuilding = false;
            var buildingHeight = DefaultBuildingHeight;
            var buildingObjects = new List<BuildingObject>();
            //find tags in 'way' tag
            foreach (XmlNode wayTag in wayTags)
            {
                //node is 'nd' or 'tag' tags
                for (var i = wayTag.ChildNodes.Count - 1; i >= 0; i--)
                {
                    //<nd...> <tag....>
                    var node = wayTag.ChildNodes[i];
                    // attributes in 'k=' attribute
                    if (node.Name.Equals("tag"))
                    {
                        var tagKeyValue = node.Attributes["k"].Value;
                        //is object a building?
                        if (tagKeyValue.Equals("building"))
                            isBuilding = true;

                        //get building height
                        if (tagKeyValue.Equals("building:levels"))
                        {
                            buildingHeight = float.Parse(node.Attributes["v"].Value);
                        }

                        //neprocházet zbytečně <nd tagy
                        if (isBuilding && buildingHeight > DefaultBuildingHeight)
                            break;
                    }
                }

                if (isBuilding)
                {
                    var building = GetCoordinatesById(nodeTags, nodeTagIds, wayTag, buildingHeight);
                    //assign address to building object
                    AssignBuildingAddresses(building, addressObjects);
                    buildingObjects.Add(building);
                    //reset values for next iteration
                    isBuilding = false;
                    buildingHeight = DefaultBuildingHeight;
                }
            }

            print("Building count " + buildingObjects.Count);
            return buildingObjects;
        }

        /**
         * 
         */
        private BuildingObject GetCoordinatesById(XmlNodeList nodeTags, List<long> nodeTagIds, XmlNode wayTag, float buildingHeight)
        {
            if (buildingHeight == DefaultBuildingHeight)
                buildingHeight = 1;

            //ids for coordinate pairing from way tag
            var locationIDs = LoaderUtils.GetWayTagIDs(wayTag);
            var latLngObjects = new LatLngObject[locationIDs.Count];
            var numOfFoundedIDs = 0;
            for (var k = 0; k < nodeTags.Count; k++)
            {
                //cached id from node tag
                var id = nodeTagIds[k];
                int locationIndex;
                if ((locationIndex = locationIDs.IndexOf(id)) != -1)
                {
                    var nodeTag = nodeTags[k];
                    var lat = float.Parse(nodeTag.Attributes["lat"].Value);
                    var lon = float.Parse(nodeTag.Attributes["lon"].Value);
                    latLngObjects[locationIndex] = new LatLngObject(lat, lon);
                    ++numOfFoundedIDs;
                }

                //pokud všechny souřadnice byly spárovány s IDčkama od budovy
                if (locationIDs.Count == numOfFoundedIDs)
                    break;
            }

            return new BuildingObject(latLngObjects.ToList(), buildingHeight);
        }

        /**
         * 
         */
        private void AssignBuildingAddresses(BuildingObject building, List<AddressObject> addressObjects)
        {
            var buildingList = new List<BuildingObject> {building};
            var middleBuildingPoint = TerrainRender.GetMiddlePoint(buildingList);
            AddressObject finalAddress = null;
            var finalDiffLat = float.MaxValue;
            var finalDiffLon = float.MaxValue;
            foreach (var address in addressObjects)
            {
                var diffLat = Math.Abs(middleBuildingPoint.Latitude - address.Position.Latitude);
                var diffLon = Math.Abs(middleBuildingPoint.Longitude - address.Position.Longitude);
                if (diffLat < finalDiffLat && diffLon < finalDiffLon)
                {
                    finalAddress = address;
                    finalDiffLon = diffLon;
                    finalDiffLat = diffLat;
                }
            }

            building.AddressObject = finalAddress;
        }
    }
}