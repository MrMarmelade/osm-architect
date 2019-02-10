using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DataObjects;
using UnityEngine;
using Utils;

namespace Parser
{
    /**
     * Parser for ways like 'highway' and 'railway' tags
     */
    public class RoadLoader : MonoBehaviour
    {
        //SINGLETON
        private static RoadLoader roadLoader;


        /**
         * Singleton for road loader
         */
        public static RoadLoader Get()
        {
            if (roadLoader == null)
                roadLoader = (new GameObject("RoadLoader")).AddComponent<RoadLoader>();
            return roadLoader;
        }

        /**
         * 
         */
        public List<RoadObject> LoadFile(XmlNodeList nodeTags, List<long> nodeTagIds, XmlNodeList wayTags)
        {
            return ParseRoads(nodeTags, nodeTagIds, wayTags);
        }

        /**
         * Parse data => call XML file parsing
         */
        private List<RoadObject> ParseRoads(XmlNodeList nodeTags, List<long> nodeTagIds, XmlNodeList wayTags)
        {
            var isRoad = false;
            var vehicleType = RoadUtils.VehicleUndefined;
            var roadType = 0;

            List<RoadObject> roads = new List<RoadObject>();
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
                        var nodeKeyValue = node.Attributes["k"].Value;
                        //remove tunnels
                        if (nodeKeyValue.Equals("tunnel"))
                            break;

                        //is object a road? and get its roadType
                        if (nodeKeyValue.Equals("highway") && node.Attributes["v"] != null)
                        {
                            roadType = RoadUtils.GetRoadType(node.Attributes["v"].Value);
                            isRoad = true;
                            vehicleType = RoadUtils.VehicleCar;
                            break;
                        }

                        //tram road
                        if (nodeKeyValue.Equals("railway") && node.Attributes["v"].Value.Equals("tram"))
                        {
                            isRoad = true;
                            vehicleType = RoadUtils.VehicleTram;
                            break;
                        }
                    }
                }

                if (isRoad)
                {
                    roads.Add(GetCoordinatesById(nodeTags, nodeTagIds, wayTag, vehicleType, roadType));
                    //reset values for next iteration
                    isRoad = false;
                    vehicleType = RoadUtils.VehicleUndefined;
                }
            }

            Debug.Log("Road count: " + roads.Count);
            return roads;
        }

        /**
         * 
         */
        private RoadObject GetCoordinatesById(XmlNodeList nodeTags, List<long> nodeTagIds, XmlNode wayTag, int vehicleType, int roadType)
        {
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

            var roadObject = new RoadObject
            {
                LatLngCoordinates = latLngObjects.ToList(),
                RoadType = roadType,
                VehicleType = vehicleType
            };
            return roadObject;
        }
    }
}