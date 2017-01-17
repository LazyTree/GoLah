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
        #region Fields

        private const string URI = @"http://datamall2.mytransport.sg/ltaodataservice/";
        private const string key = @"4DZEmxtLQOmpRFW8vgqmTA==";
        private const string accept = "application/json";

        private const string BUS_STOPS = "BusStops";
        private const string BUS_SERVICES = "BusServices";
        private const string BUS_ARRIVAL = "BusArrival?BusStopID={0}";
        private const string PAGING_SKIP = @"?$skip={0}";
        private const int PAGE_SIZE = 50;

        private static List<BusRoute> _cachedBusRoutes = new List<BusRoute>();
        private static List<BusStop> _cachedBusStops = new List<BusStop>();

        #endregion

        #region Properties

        /// <summary>
        /// Cached bus stops.
        /// </summary>
        public List<BusStop> CachedBusStops
        {
            get
            {
                if(!_cachedBusStops.Any())
                {
                    _cachedBusStops = Task.Run(() => GetBusStopsAsync()).Result.ToList();
                }
                return _cachedBusStops;
            }
        }

        /// <summary>
        /// Cached bus services.
        /// </summary>
        public List<BusRoute> CachedRoutes
        {
            get
            {
                if (!_cachedBusRoutes.Any())
                {
                    _cachedBusRoutes = Task.Run(() => GetBusRoutesAsync()).Result.ToList();
                }

                return _cachedBusRoutes;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all bus stops.
        /// </summary>
        /// <param name="useCache">True to get cached result. False to get fresh result.</param>
        /// <returns></returns>
        public async Task<IEnumerable<BusStop>> GetBusStopsAsync(bool useCache = true)
        {
            if (useCache && _cachedBusStops.Any())
            {
                return _cachedBusStops;
            }

            int page = 0;
            IEnumerable<BusStop> result;

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
        /// Get the all bus routes.
        /// </summary>
        /// <param name="useCache">True to get cached result. False to get fresh result.</param>
        /// <returns></returns>
        public async Task<IEnumerable<BusRoute>> GetBusRoutesAsync(bool useCache = true)
        {
            if(useCache && _cachedBusRoutes.Any())
            {
                return _cachedBusRoutes;
            }

            int page = 0;
            IEnumerable<BusRoute> result;
            _cachedBusRoutes.Clear();
            do
            {
                result = await GetBusRoutesByPageAsync(page);
                _cachedBusRoutes.AddRange(result.ToList());
                page += PAGE_SIZE;
            }
            while (result.Count() == PAGE_SIZE);

            return _cachedBusRoutes;
        }

        /// <summary>
        /// Get the bus service by page (50 records per page)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BusRoute>> GetBusRoutesByPageAsync(int page)
        {
            var jsonString = await GetResponseStringAsync(string.Concat(URI, BUS_SERVICES, page == 0 ? string.Empty : string.Format(PAGING_SKIP, page)));
            var pattern = "(FLAT FARE \\$[0-9]+(?:\\.[0-9][0-9])?)(?![\\d])";
            jsonString = Regex.Replace(jsonString, pattern, "FlatFee");
            return JsonConvert.DeserializeObject<OData<BusRoute>>(jsonString)?.Value;
        }

        /// <summary>
        /// Get bus arrival info of the specified bus stop.
        /// </summary>
        /// <param name="busStopId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ArrivalBusService>> GetNextBusAsync(string busStopId)
        {
            var jsonString = await GetResponseStringAsync(string.Concat(URI, string.Format(BUS_ARRIVAL, busStopId)));
            return JsonConvert.DeserializeObject<BusArrivalOData>(jsonString)?.Services;
        }

        /// <summary>
        /// Get bus stop description by bus stop code for UI binding converter.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public BusStop GetBusStopByCode(string code)
        {
            return CachedBusStops.SingleOrDefault(x => x.Code.Equals(code));
        }

        /// <summary>
        /// Get the Json response from LTA datamall REST API.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
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

        #endregion
    }
}
