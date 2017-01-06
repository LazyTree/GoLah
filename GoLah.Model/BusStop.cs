using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace GoLah.Model
{
    public class BusStop
    {
        private string _code;
        private string _roadName;
        private string _description;
        private Geocoordinate _location;
        
        public BusStop(string code, string roadName, string description, Geocoordinate location)
        {
            _code = code;
            _roadName = roadName;
            _description = description;
            _location = location;
        }

        public string Code
        {
            get
            {
                return _code;
            }
        }

        public string RoadName
        {
            get
            {
                return _roadName;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public Geocoordinate Location
        {
            get
            {
                return _location;
            }
        }
    }
}