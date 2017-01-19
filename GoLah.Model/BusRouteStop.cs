using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    public class BusRouteStop
    {
        public string ServiceNo { get; set; }
        public string Operator { get; set; }
        public byte Direction { get; set; }
        public int StopSequence { get; set; }
        public string BusStopCode { get; set; }
        public string Distance { get; set; }
        [JsonProperty("WD_FirstBus")]
        public string WeekDayFirstBusTime { get; set; }
        [JsonProperty("WD_LastBus")]
        public string WeekDayLastBusTime { get; set; }
        [JsonProperty("SAT_FirstBus")]
        public string SaturdayFirstBusTime { get; set; }
        [JsonProperty("SAT_LastBus")]
        public string SaturdayLastBusTime { get; set; }
        [JsonProperty("SUN_FirstBus")]
        public string SundayFirstBusTime { get; set; }
        [JsonProperty("SUN_LastBus")]
        public string SundayLastBusTime { get; set; }
    }
}
