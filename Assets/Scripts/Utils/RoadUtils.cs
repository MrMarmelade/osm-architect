using UnityEngine;

namespace Utils
{
    /**
     * Metody pro práci s různými typy cest podle (seznam typů cest):
     * @see https://wiki.openstreetmap.org/wiki/Key:highway
     */
    public static class RoadUtils
    {
        //Vehicle types
        public const int VehicleUndefined = 0;
        public const int VehicleCar = 1;
        public const int VehicleTram = 2;

        //OSM Road types
        public const int Residential = 1;
        public const int Service = 2;
        private const int Footway = 3;
        private const int Track = 4;
        private const int Escape = 5;
        private const int Bridleway = 6;
        private const int Steps = 7;
        private const int Path = 8;
        private const int Cycleway = 9;
        private const int LivingStreet = 10;
        private const int Motorway = 11;
        private const int Trunk = 12;
        private const int Primary = 13;
        public const int Secondary = 14;
        private const int Tertiary = 15;
        private const int Unclassified = 16;


        /**
         *  Vrací konstantu podle typu cesty
         */
        public static int GetRoadType(string roadTypeValue)
        {
            switch (roadTypeValue)
            {
                case "motorway":
                    return Motorway;
                case "trunk":
                    return Trunk;
                case "primary":
                    return Primary;
                case "secondary":
                    return Secondary;
                case "tertiary":
                    return Tertiary;
                case "unclassified":
                    return Unclassified;
                case "residential":
                    return Residential;
                case "service":
                    return Service;
                case "footway":
                    return Footway;
                case "track":
                    return Track;
                case "escape":
                    return Escape;
                case "bridleway":
                    return Bridleway;
                case "steps":
                    return Steps;
                case "path":
                    return Path;
                case "cycleway":
                    return Cycleway;
                case "living_street":
                    return LivingStreet;
                default:
                    return 0;
            }
        }

        /**
         * Podle konstanty daného typu cesty vrací šířku dané cesty
         */
        public static float GetRoadWidth(int roadType)
        {
            switch (roadType)
            {
                case Residential:
                    return 0.1f;
                case Service:
                    return 0.1f;
                case Footway:
                    return 0.05f;
                case Track:
                    return 0.1f;
                case Escape:
                    return 0.1f;
                case Bridleway:
                    return 0.1f;
                case Steps:
                    return 0.05f;
                case Path:
                    return 0.04f;
                case Cycleway:
                    return 0.06f;
                case LivingStreet:
                    return 0.1f;
                default:
                    return 0.2f;
            }
        }

        /**
         *  Podle konstanty daného typu cesty vrací její barvu
         */
        public static Color GetRoadColor(int roadType)
        {
            switch (roadType)
            {
                case Residential:
                    return Color.magenta;
                case Service:
                    return Color.white;
                case Footway:
                    return Color.yellow;
                default:
                    return Color.blue;
            }
        }
    }
}