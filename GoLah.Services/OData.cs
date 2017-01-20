using GoLah.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Services
{
    public class OData<T> where T : LtaData
    {
        [JsonProperty("odata.metadata")]
        public string Metadata { get; set; }

        [JsonProperty("Services")]
        public List<T> Service { get; set; }

        [JsonProperty("Value")]
        public List<T> Value { get; set; }

        [JsonProperty("BusStopID")]
        [JsonIgnore]
        public string BusStopID { get; set; }
    }
}
