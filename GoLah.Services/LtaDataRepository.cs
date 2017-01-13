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

        private List<BusRoutine> _cachedBusRoutines = new List<BusRoutine>();
        private static List<BusService> _cachedBusServices = new List<BusService>();
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
        public List<BusService> CachedBusServices
        {
            get
            {
                if (!_cachedBusServices.Any())
                {
                    _cachedBusServices = Task.Run(() => GetBusServicesAsync()).Result.ToList();
                }
                return _cachedBusServices;
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
        public async Task<IEnumerable<BusService>> GetBusServicesAsync(bool useCache = true)
        {
            if(useCache && _cachedBusServices.Any())
            {
                return _cachedBusServices;
            }

            int page = 0;
            IEnumerable<BusRoutine> result;
            _cachedBusServices.Clear();
            do
            {
                result = await GetBusRoutinesByPageAsync(page);
                _cachedBusRoutines.AddRange(result.ToList());
                page += PAGE_SIZE;
            }
            while (result.Count() == PAGE_SIZE);
            _cachedBusServices = MergeBusRoutineToBusService(_cachedBusRoutines).ToList();
            _cachedBusServices.Sort();
            return _cachedBusServices;
        }

        /// <summary>
        /// Get the bus service by page (50 records per page)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BusRoutine>> GetBusRoutinesByPageAsync(int page)
        {
            var jsonString = await GetResponseStringAsync(string.Concat(URI, BUS_SERVICES, page == 0 ? string.Empty : string.Format(PAGING_SKIP, page)));
            var pattern = "(FLAT FARE \\$[0-9]+(?:\\.[0-9][0-9])?)(?![\\d])";
            jsonString = Regex.Replace(jsonString, pattern, "FlatFee");
            return JsonConvert.DeserializeObject<OData<BusRoutine>>(jsonString)?.Value;
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

        /// <summary>
        /// Massage the bus routines to bus services.
        /// </summary>
        /// <param name="_cachedBusRoutines"></param>
        /// <returns></returns>
        private IEnumerable<BusService> MergeBusRoutineToBusService(List<BusRoutine> _cachedBusRoutines)
        {
            var groups = _cachedBusRoutines.GroupBy(x => x.ServiceNo);
            foreach (var group in groups)
            {
                yield return new BusService()
                {
                    ServiceNo = group.Key,
                    Routines = group.Select(x => x).ToArray()
                };
            }
        }

        #endregion
    }
}
