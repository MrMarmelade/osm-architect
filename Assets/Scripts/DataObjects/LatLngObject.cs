namespace DataObjects
{
    /**
     * 
     */
    public class LatLngObject
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }


        public LatLngObject()
        {
        }

        public LatLngObject(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }


        protected bool Equals(LatLngObject other)
        {
            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LatLngObject) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Latitude.GetHashCode() * 397) ^ Longitude.GetHashCode();
            }
        }
    }
}