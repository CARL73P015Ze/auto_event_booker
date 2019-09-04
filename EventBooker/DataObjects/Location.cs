using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObjects
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location()
        {
            Latitude = 0;
            Longitude = 0;
        }

    }
}
