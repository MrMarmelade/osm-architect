using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DataObjects;
using UnityEngine;
using Logger = Utils.Logger;

namespace Parser
{
    /**
     * 
     */
    public class AltitudeLoader : MonoBehaviour
    {
        //SINGLETON
        private static AltitudeLoader altitudeLoader;

        public List<ElevationObject> AltitudeData;
        private ArrayList LocalAltitudeData;

        private byte AcceptedRequests;

        private const byte NumberOfRequests = 4;


        /**
         * Singleton for altitude loader
         */
        public static AltitudeLoader Get()
        {
            if (altitudeLoader == null)
                altitudeLoader = (new GameObject("AltitudeLoader")).AddComponent<AltitudeLoader>();

            return altitudeLoader;
        }

        /**
        * Set altitude parameter into @param buildingObjects
        */
        public void SetAltitude(List<LatLngObject> latLngObjects)
        {
            if (!CheckDownloadOption())
            {
                return;
            }

            var ApiKey = "&key=" + Main.Key.ApiKey;
            AltitudeData = new List<ElevationObject>();
            LocalAltitudeData = new ArrayList();
            AcceptedRequests = 0;

            var entitiesPerRequest = (latLngObjects.Count / NumberOfRequests) + 1;
            var start = 0;
            var end = entitiesPerRequest;
            for (var i = 1; i <= NumberOfRequests; i++)
            {
                if (start + end >= latLngObjects.Count)
                {
                    end = latLngObjects.Count - start;
                }

                var latLngPerRequest = latLngObjects.GetRange(start, end);
                //create api altitude request
                var stringBuilder = new StringBuilder("https://maps.googleapis.com/maps/api/elevation/xml?locations=");
                stringBuilder.Append(i + ".0");
                stringBuilder.Append(",");
                stringBuilder.Append(i + ".0");
                stringBuilder.Append("|");
                foreach (var latLngObject in latLngPerRequest)
                {
                    stringBuilder.Append(latLngObject.Latitude);
                    stringBuilder.Append(",");
                    stringBuilder.Append(latLngObject.Longitude);
                    stringBuilder.Append("|");
                }

                //remove pipe at the end
                stringBuilder.Length -= 1;
                //append api key
                stringBuilder.Append(ApiKey);
                //download and set altitude
                GetAltitudes(stringBuilder);

                start += end;
                end = entitiesPerRequest;
            }
        }

        /**
         * 
         */
        private bool CheckDownloadOption()
        {
            if (!Main.CanRenderObjects)
            {
                return true;
            }

            return false;
        }

        /**
         * Download the XML with altitude for one of LanLng building coordinate
         */
        private void GetAltitudes(StringBuilder apiRequest)
        {
            try
            {
                var www = new WWW(apiRequest.ToString());
                StartCoroutine("WaitForWWW", www);
            }
            catch (UnityException unityException)
            {
                Debug.Log(unityException.Message);
            }
        }

        /**
         * Wait for download of the XML with altitude for one of LanLng building coordinate
         */
        IEnumerator WaitForWWW(WWW www)
        {
            Main.CanRenderObjects = false;
            yield return www;
            string result;
            //server response
            if (string.IsNullOrEmpty(www.error))
            {
                result = www.text;
                ParseResult(result);
            }
            //error
            else
            {
                result = www.error;
                Logger.Print(result);
            }

            yield return result;
        }

        /**
         * 
         */
        private void ParseResult(String xmlResult)
        {
            var xmlFile = new XmlDocument();
            xmlFile.LoadXml(xmlResult);
            var elevationNodes = xmlFile.GetElementsByTagName("elevation");
            var latNodes = xmlFile.GetElementsByTagName("lat");
            var lngNodes = xmlFile.GetElementsByTagName("lng");

            lock (LocalAltitudeData.SyncRoot)
            {
                //1 = skip request number
                for (var i = 0; i < elevationNodes.Count; i++)
                {
                    var elevationObject = new ElevationObject
                    {
                        Elevation = float.Parse(elevationNodes[i].InnerText),
                        Latitude = double.Parse(latNodes[i].InnerText),
                        Longitude = double.Parse(lngNodes[i].InnerText)
                    };
                    LocalAltitudeData.Add(elevationObject);
                }

                ++AcceptedRequests;
                if (AcceptedRequests == NumberOfRequests)
                {
                    AltitudeData = OrderRequestData(LocalAltitudeData);
                }
            }
        }

        /**
         * Order server response data by first fictive coordinate in every response
         */
        private List<ElevationObject> OrderRequestData(ArrayList elevationObjects)
        {
            var orderedData = new List<ElevationObject>();
            for (var i = 1; i <= NumberOfRequests; i++)
            {
                var add = false;
                foreach (var elevationObject in elevationObjects)
                {
                    var currentElevation = (ElevationObject) elevationObject;
                    var roundedLat = (int) Math.Round(currentElevation.Latitude, 0);
                    var roundedLng = (int) Math.Round(currentElevation.Longitude, 0);
                    if (roundedLat > 0 && roundedLat <= NumberOfRequests && roundedLng > 0 && roundedLng <= NumberOfRequests && add)
                        break;

                    if (add)
                        orderedData.Add(currentElevation);

                    if (roundedLat == i && roundedLng == i)
                        add = true;
                }
            }

            //run object render in Main
            Main.CanRenderObjects = true;
            return orderedData;
        }
    }
}