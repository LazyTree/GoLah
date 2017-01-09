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
        public int VisitNumber { get; set; }

        /// <summary>
        /// ETA date-time.
        /// </summary>
        public DateTime EstimateArrival { get; set; }

        /// <summary>
        /// Estimated location coordinates of bus at point of published data.
        /// Values will be “0” if bus has yet to leave the interchange.
        /// </summary>
        public Geocoordinate Location { get; set; }

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
