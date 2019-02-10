namespace DataObjects
{
    /**
     * 
     */
    public class AddressObject
    {
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public LatLngObject Position { get; set; }


        protected bool Equals(AddressObject other)
        {
            return string.Equals(Street, other.Street) && string.Equals(StreetNumber, other.StreetNumber) && Equals(Position, other.Position);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AddressObject) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Street != null ? Street.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StreetNumber != null ? StreetNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Position != null ? Position.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}