using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    public class BusService
    {
        public string ServiceNumber { get; set; }

        public string Operator { get; set; }

        public BusStop OriginalBusStop { get; set; }

        public BusStop TerminatingBusStop { get; set; }

        public List<NextBus> NextBuses { get; set; }
    }
}
