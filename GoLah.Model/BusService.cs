using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    public class BusService : IComparable
    {
        #region Properties

        public string ServiceNo { get; set; }

        public string Operator { get; set; }

        public BusDirection[] Directions { get; set; }

        public bool IsLoop
        {
            get
            {
                return Directions.Length == 1;
            }
        }

        public string LoopDescription { get; set; }

        public int CompareTo(object obj)
        {
            return ServiceNo.CompareTo((obj as BusService).ServiceNo);
        }
        #endregion
    }
}
