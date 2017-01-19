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
    /// Next approaching bus information.
    /// </summary>
    public class NextBus
    {
        /// <summary>
        /// Ordinal value of the nth visit of this vehicle at this bus stop; 1=1st visit, 2=2nd visit.
        /// </summary>
        public string VisitNumber { get; set; }

        /// <summary>
        /// ETA date-time.
        /// </summary>
        public DateTime? EstimatedArrival { get; set; }

        /// <summary>
        /// Estimated location Latitude of bus at point of published data.
        /// Values will be “0” if bus has yet to leave the interchange.
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Estimated location Longitude of bus at point of published data.
        /// Values will be “0” if bus has yet to leave the interchange.
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Bus occupancy/crowding.
        /// </summary>
        public string Load { get; set; }

        /// <summary>
        /// Indicates wheel-chair accessible bus.
        /// </summary>
        public string Feature { get; set; }
    }
}
