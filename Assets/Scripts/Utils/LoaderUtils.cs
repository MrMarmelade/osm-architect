using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Utils
{
    /**
     * Utils for XML Loaders
     */
    public static class LoaderUtils
    {
        /**
         * Get IDs from 'way' tag (using later for pairing with tags which contains LatLng attributes)
         */
        public static List<long> GetWayTagIDs(XmlNode wayTag)
        {
            var locationIDs = new List<long>();
            foreach (XmlNode nd in wayTag)
            {
                XmlAttribute refAttr;
                if ((refAttr = nd.Attributes["ref"]) != null)
                    locationIDs.Add(long.Parse(refAttr.Value));
            }

            //remove duplicates ref numbers and return it
            return locationIDs.Distinct().ToList();
        }
    }
}