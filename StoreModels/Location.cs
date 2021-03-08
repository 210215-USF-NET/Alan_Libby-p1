using System;

namespace StoreModels
{
    public class Location
    {
        private string locationName;
        public int LocationId { get; set; }
        public string LocationName { get { return locationName; }
            set {
                if (value == null || value.Equals("")) {
                    throw new ArgumentNullException("Cannot set LocationName to null or empty.");
                }
                locationName = value;
            }
        }
        public string LocationAddress { get; set; }
    }
}
