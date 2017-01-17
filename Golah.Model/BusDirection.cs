using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    /// <summary>
    /// Bus direction defined by a list of stops.
    /// </summary>
    public class BusDirection
    {
        public byte Direction { get; set; }

        public List<BusStop> Stops { get; set; }

        public BusTiming Timing { get; set; }

        //public string Origin
        //{
        //    get
        //    {
        //       return Stops.FirstOrDefault().Description;
        //    }
        //}

        //public string Destination
        //{
        //    get
        //    {
        //        return Stops.LastOrDefault().Description;
        //    }
        //}

        public string Origin { get; set; }

        public string Destination { get; set; }
    }
}
