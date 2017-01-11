using GoLah.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoLah.Services
{
    public class LtaDataRepository
    {
        private const string URI = @"http://datamall2.mytransport.sg/ltaodataservice/";
        private const string key = @"4DZEmxtLQOmpRFW8vgqmTA==";
        private const string accept = "application/json";

        private const string BUS_STOPS = "BusStops";
        private const string BUS_SERVICES = "BusServices";
        private const string BUS_ARRIVAL = "BusArrival?BusStopID={0}";
        private const string PAGING_SKIP = @"?$skip={0}";
        private const int PAGE_SIZE = 50;

        private List<BusService> _cachedBusServices = new List<BusService>();
        private List<BusStop> _cachedBusStops = new List<BusStop>();

        private async Task<string> GetResponseStringAsync(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            var headers = new WebHeaderCollection();
            headers["AccountKey"] = key;
            headers["Accept"] = accept;
            httpWebRequest.Headers = headers;
            httpWebRequest.Method = "GET";

            var response = await httpWebRequest.GetResponseAsync();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }

        /// <summary>
        /// Get all bus stops.
        /// </summary>
        /// <param name="useCache">True to get cached result. False to get fresh result.</param>
        /// <returns></returns>
        public async Task<IEnumerable<BusStop>> GetBusStopsAsync(bool useCache)
        {
            if (useCache && _cachedBusStops.Count > 0)
            {
                return _cachedBusStops;
            }

            int page = 0;
            IEnumerable<BusStop> result;
            _cachedBusServices.Clear();
            do
            {
                result = await GetBusStopsByPageAsync(page);
                _cachedBusStops.AddRange(result);
                page += PAGE_SIZE;
            }
            while (result.Count() == PAGE_SIZE);
            return _cachedBusStops;
        }

        /// <summary>
        /// Get the bus stops by page (50 records per page)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BusStop>> GetBusStopsByPageAsync(int page)
        {
            var jsonString = await GetResponseStringAsync(string.Concat(URI, BUS_STOPS, page == 0 ? string.Empty : string.Format(PAGING_SKIP, page)));
            return JsonConvert.DeserializeObject<OData<BusStop>>(jsonString)?.Value;
        }

        /// <summary>
        /// Get the all bus services.
        /// </summary>
        /// <param name="useCache">True to get cached result. False to get fresh result.</param>
        /// <returns></returns>
        public async Task<IEnumerable<BusService>> GetBusServicesAsync(bool useCache)
        {
            if(useCache && _cachedBusServices.Count > 0)
            {
                return _cachedBusServices;
            }

            int page = 0;
            IEnumerable<BusService> result;
            _cachedBusServices.Clear();
            do
            {
                result = await GetBusServicesByPageAsync(page);
                _cachedBusServices.AddRange(result.ToList());
                page += PAGE_SIZE;
            }
            while (result.Count() == PAGE_SIZE);
            return _cachedBusServices;
        }

        /// <summary>
        /// Get the bus service by page (50 records per page)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BusService>> GetBusServicesByPageAsync(int page)
        {
            var jsonString = await GetResponseStringAsync(string.Concat(URI, BUS_SERVICES, page == 0 ? string.Empty : string.Format(PAGING_SKIP, page)));
            var pattern = "(FLAT FARE \\$[0-9]+(?:\\.[0-9][0-9])?)(?![\\d])";
            jsonString = Regex.Replace(jsonString, pattern, "FlatFee");
            return JsonConvert.DeserializeObject<OData<BusService>>(jsonString)?.Value;
        }

        public async Task<IEnumerable<ArrivalBusService>> GetNextBusAsync(string busStopId)
        {
            var jsonString = await GetResponseStringAsync(string.Concat(URI, string.Format(BUS_ARRIVAL, busStopId)));
            return JsonConvert.DeserializeObject<BusArrivalOData>(jsonString)?.Services;
        }
    }
}
