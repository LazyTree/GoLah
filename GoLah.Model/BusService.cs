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

        /// <summary>
        /// The bus service number.
        /// </summary>
        public string ServiceNo { get; set; }

        public BusRoutine[] Routines { get; set; }

        public int CompareTo(object obj)
        {
            return ServiceNo.CompareTo((obj as BusService).ServiceNo);
        }
        #endregion
    }
}
