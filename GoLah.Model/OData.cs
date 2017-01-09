using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    public class OData<T>
    {
        [JsonProperty("odata.metadata")]
        public string Metadata { get; set; }
        public List<T> Value { get; set; }
    }

    public class BusArrivalOData
    {
        [JsonProperty("odata.metadata")]
        public string Metadata { get; set; }
        public string BusStopID { get; set; }
        public List<ArrivalBusService> Services { get; set; }
    }
}
