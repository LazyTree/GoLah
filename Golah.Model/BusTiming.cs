using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Model
{
    public class BusTiming
    {
        /// <summary>
        /// Frequence of dispatch for AM Peak 0630H - 0830H (range in minutes).
        /// </summary>
        public string MorningPeakFrequency { get; set; }

        /// <summary>
        /// Frequency of dispatch for AM Off-Peak 0831H - 1659H (range in minutes).
        /// </summary>
        public string MorningOffpeakFrequency { get; set; }

        /// <summary>
        /// Frequency of dispatch for PM Peak 1700H - 1900H (range in minutes).
        /// </summary>
        public string EveningPeakFrequency { get; set; }

        /// <summary>
        /// Frequency of dispatch for PM Off-Peak after 1900H (range in minutes).
        /// </summary>
        public string EveningOffPeakFrequency { get; set; }
    }
}
