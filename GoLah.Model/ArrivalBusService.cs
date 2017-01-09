using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    /// <summary>
    /// Arrival bus service information.
    /// </summary>
    public class ArrivalBusService
    {
        /// <summary>
        /// The bus service number.
        /// </summary>
        public string ServiceNo { get; set; }

        /// <summary>
        /// Transport operator code.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Reference code for first bus stop in this service’s route sequence.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Bus stop code for first bus stop.
        /// </summary>
        public string OriginatingID { get; set; }

        /// <summary>
        /// Reference code for last bus stop in this service’s route sequence.
        /// </summary>
        public string TerminatingID { get; set; }

        /// <summary>
        /// Next approaching bus.
        /// </summary>
        public NextBus NextBus { get; set; }

        /// <summary>
        /// Subsequent bus.
        /// </summary>
        public NextBus SubsequentBus { get; set; }

        /// <summary>
        /// Third oncoming bus .
        /// </summary>
        public NextBus SubsequentBus3 { get; set; }
    }
}
