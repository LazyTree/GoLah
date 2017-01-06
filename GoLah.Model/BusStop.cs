using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    public class BusStop
    {
        public string BusStopCode { get; set; }

        public string RoadName { get; set; }

        public string Description { get; set; }

        public List<BusService> BusServices { get; set; }
    }
}
