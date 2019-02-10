using System.Collections.Generic;

namespace DataObjects
{
    /**
     * Parent for rendered objects(buildings, roads)
     */
    public class RenderedObjectParent
    {
        public List<LatLngObject> LatLngCoordinates { get; set; }
    }
}