using System.Collections.Generic;
using UnityEngine;

namespace DataObjects
{
    /**
     * Data pro tvorbu 3D objektu. Obsahuje jednotlivé XYZ souřadnice tvořící daný objekt spolu s jeho výškou.
     */
    public class ThreeDimObject
    {
        public List<Vector3> XyzCoordinates { get; set; }
        public float Height { get; set; }

        public string Name { get; set; }
        public AddressObject Address { get; set; }


        public ThreeDimObject(List<Vector3> xyzCoordinates)
        {
            XyzCoordinates = xyzCoordinates;
        }
        
        public ThreeDimObject(List<Vector3> xyzCoordinates, float height, AddressObject address)
        {
            XyzCoordinates = xyzCoordinates;
            Height = height;
            Address = address;
        }
    }
}