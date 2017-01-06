using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    public class NextBus
    {
        public int VisitNumber { get; set; }

        public DateTime EstimateArrival { get; set; }

        public GeoLocation Location { get; set; }

        public Load Load { get; set; }

        public Feature Feature { get; set; }

        public Status Status { get; set; }
    }
}
