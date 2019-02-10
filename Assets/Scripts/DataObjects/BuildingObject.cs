using System.Collections.Generic;

namespace DataObjects
{
    /**
     * Object which contains coordinates of building in lat/lng format and its height
     */
    public class BuildingObject : RenderedObjectParent
    {
        public float BuildingHeight { get; set; }
        public AddressObject AddressObject { get; set; }


        public BuildingObject(List<LatLngObject> latLngObjects, float buildingHeight)
        {
            LatLngCoordinates = latLngObjects;
            BuildingHeight = buildingHeight;
        }
    }
}