using UnityEngine;

namespace DataObjects
{
    /**
     * 
     */
    public class RoadObject : RenderedObjectParent
    {
        /**
         * 
         */
        public int RoadType { get; set; }

        /**
         * Parameter from RoadUtils/parser, road for cars/trams etc.
         */
        public int VehicleType { get; set; }

        /**
         * 
         */
        public Vector3[] XyzCoordinates { get; set; }

        /**
         * 
         */
        public int CurrentIndexPosition { get; set; }
    }
}