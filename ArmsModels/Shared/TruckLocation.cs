using System;

namespace ArmsModels.Shared
{
    public class TruckLocation
    {
        public double Altitude { get; set; }
        public DateTime Datetime { get; set; }
        public int Odometer { get; set; }
        public int Heading { get; set; }
        public string Vehicleregnumber { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Batlevel { get; set; }
        public int Ignition { get; set; }
        public double Speed { get; set; }
        public double Longitude { get; set; }
    }
}
