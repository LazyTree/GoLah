﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    /// <summary>
    /// Bus service information.
    /// </summary>
    public class BusRoute : LtaData
    {
        #region Properties

        /// <summary>
        /// The bus service number.
        /// </summary>
        public string ServiceNo { get; set; }

        /// <summary>
        /// Operator for this bus service.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// The direction in which the bus travels(1 or 2), loop services only have 1 direction.
        /// </summary>
        public byte Direction { get; set; }

        /// <summary>
        /// Category of the SBS bus service.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public BusServiceCategory Category { get; set; }

        /// <summary>
        /// Bus stop code for first bus stop.
        /// </summary>
        public string OriginCode { get; set; }

        /// <summary>
        /// Bus stop code for last bus stop(similar as first stop for loop services).
        /// </summary>
        public string DestinationCode { get; set; }

        /// <summary>
        /// Frequence of dispatch for AM Peak 0630H - 0830H (range in minutes).
        /// </summary>
        [JsonProperty("AM_Peak_Freq")]
        public string MorningPeakFrequency { get; set; }

        /// <summary>
        /// Frequency of dispatch for AM Off-Peak 0831H - 1659H (range in minutes).
        /// </summary>
        [JsonProperty("AM_Offpeak_Freq")]
        public string MorningOffpeakFrequency { get; set; }

        /// <summary>
        /// Frequency of dispatch for PM Peak 1700H - 1900H (range in minutes).
        /// </summary>
        [JsonProperty("PM_Peak_Freq")]
        public string EveningPeakFrequency { get; set; }

        /// <summary>
        /// Frequency of dispatch for PM Off-Peak after 1900H (range in minutes).
        /// </summary>
        [JsonProperty("PM_Offpeak_Freq")]
        public string EveningOffPeakFrequency { get; set; }

        /// <summary>
        /// Location at which the bus service loops, empty if not a loop service.
        /// </summary>
        [JsonProperty("LoopDesc")]
        public string LoopDescription { get; set; }

        /// <summary>
        /// Codes of bus stops along the route.
        /// </summary>
        public List<string> BusStopCodes { get; set; }

        public override string ServiceUrl => "BusServices";

        #endregion
    }
}
