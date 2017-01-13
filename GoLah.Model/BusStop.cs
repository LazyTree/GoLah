using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace GoLah.Model
{
    /// <summary>
    /// Bus stop information.
    /// </summary>
    public class BusStop
    {
        #region Properties

        /// <summary>
        /// The unique 5-digit identifier for this physical bus stop.
        /// </summary>
        [JsonProperty("BusStopCode")]
        public string Code { get; set; }

        /// <summary>
        /// The road on which this bus stop is located.
        /// </summary>
        public string RoadName { get; set; }

        /// <summary>
        /// Landmarks next to the bus stop (if any) to aid in identifying this bus stop.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Location coordinates for this bus stop.
        /// </summary>
        public Geocoordinate Location { get; set; }

        #endregion
    }
}