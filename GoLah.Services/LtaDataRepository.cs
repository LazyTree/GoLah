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
using Windows.Storage;

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
        private const string BUS_ROUTES = "BusRoutes";
        private const string BUS_ARRIVAL = "BusArrival?BusStopID={0}";
        private const string PAGING_SKIP = @"?$skip={0}";
        private const int PAGE_SIZE = 50;

        private static List<BusRoute> _cachedBusRoutes = new List<BusRoute>();
        private static List<BusStop> _cachedBusStops = new List<BusStop>();
        private static List<BusRouteStop> _cachedBusRouteStops = new List<BusRouteStop>();

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get all bus stops.
        /// </summary>
        /// <param name="refresh">True to get fresh result online. False to get result from local file cache or RAM cache.</param>
        /// <returns></returns>
        public async Task<IEnumerable<BusStop>> GetBusStopsAsync(bool refresh = false)
        {
            if (!refresh)
            {
                if (_cachedBusStops.Any())
                {
                    return _cachedBusStops;
                }
                else
                {
                    _cachedBusStops = await LoadCacheAsync<BusStop>();
                    if (_cachedBusStops.Any())
                    {
                        return _cachedBusStops;
                    }
                }
            }

            int page = 0;
            IEnumerable<BusStop> result;
            _cachedBusStops.Clear();
            do
            {
                result = await GetBusStopsByPageAsync(page);
                _cachedBusStops.AddRange(result);
                page += PAGE_SIZE;
            }
            while (result.Count() == PAGE_SIZE);

            await SaveCacheAsync(_cachedBusStops);

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
        /// <param name="useCache">True to get fresh result online. False to get result from local file cache or RAM cache.</param>
        /// <returns></returns>
        public async Task<IEnumerable<BusRoute>> GetBusRoutesAsync(bool refresh = false)
        {
            if (!refresh)
            {
                if (_cachedBusRoutes.Any())
                {
                    return _cachedBusRoutes;
                }
                else
                {
                    _cachedBusRoutes = await LoadCacheAsync<BusRoute>();
                    if (_cachedBusRoutes.Any())
                    {
                        return _cachedBusRoutes;
                    }
                }
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

            _cachedBusRoutes.ForEach(x => x.BusStopCodes = 
                _cachedBusRouteStops.Where(s => s.ServiceNo == x.ServiceNo && s.Direction == x.Direction)
                .OrderBy(s => s.StopSequence)
                .Select(s => s.BusStopCode)
                .ToList());

            await SaveCacheAsync(_cachedBusRoutes);

            return _cachedBusRoutes;
        }

        /// <summary>
        /// Get the bus service by page (50 records per page)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BusRoute>> GetBusRoutesByPageAsync(int page)
        {
            var jsonString = await GetResponseStringAsync(string.Concat(URI, BUS_SERVICES, page == 0 ? string.Empty : string.Format(PAGING_SKIP, page)));
            var pattern = "(FLAT FARE \\$[0-9]+(?:\\.[0-9][0-9])?)(?![\\d])";
            jsonString = Regex.Replace(jsonString, pattern, "FlatFee");
            return JsonConvert.DeserializeObject<OData<BusRoute>>(jsonString)?.Value;
        }

        /// <summary>
        /// Get the all bus routes stops.
        /// </summary>
        /// <param name="useCache">True to get fresh result online. False to get result from local file cache or RAM cache.</param>
        /// <returns></returns>
        public async Task<IEnumerable<BusRouteStop>> GetBusRouteStopsAsync(bool refresh = false)
        {
            if (!refresh)
            {
                if (_cachedBusRouteStops.Any())
                {
                    return _cachedBusRouteStops;
                }
                else
                {
                    _cachedBusRouteStops = await LoadCacheAsync<BusRouteStop>();
                    if (_cachedBusRouteStops.Any())
                    {
                        return _cachedBusRouteStops;
                    }
                }
            }

            int page = 0;
            IEnumerable<BusRouteStop> result;
            _cachedBusRouteStops.Clear();
            do
            {
                result = await GetBusRouteStopsByPageAsync(page);
                _cachedBusRouteStops.AddRange(result.ToList());
                page += PAGE_SIZE;
            }
            while (result.Count() == PAGE_SIZE);

            await SaveCacheAsync(_cachedBusRouteStops);

            return _cachedBusRouteStops;
        }

        /// <summary>
        /// Get the bus service by page (50 records per page)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BusRouteStop>> GetBusRouteStopsByPageAsync(int page)
        {
            var jsonString = await GetResponseStringAsync(string.Concat(URI, BUS_ROUTES, page == 0 ? string.Empty : string.Format(PAGING_SKIP, page)));
            return JsonConvert.DeserializeObject<OData<BusRouteStop>>(jsonString)?.Value;
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
        public async Task<BusStop> GetBusStopByCodeAsync(string code)
        {
            var result = await GetBusStopsAsync().ConfigureAwait(false);
            return result.SingleOrDefault(x => x.Code.Equals(code));
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

        private async Task SaveCacheAsync<T>(List<T> _cachedItems)
        {
            var filePath = GetLocalCacheFilePath<T>();
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            using (var streamWriter = new StreamWriter(stream))
            {
                await streamWriter.WriteAsync(JsonConvert.SerializeObject(_cachedItems));
            }
        }

        private async Task<List<T>> LoadCacheAsync<T>()
        {
            var filePath = GetLocalCacheFilePath<T>();
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }
            using (var stream = new FileStream(filePath, FileMode.Open))
            using (var streamReader = new StreamReader(stream))
            {
                var jsonString = await streamReader.ReadToEndAsync();
                var pattern = "(FLAT FARE \\$[0-9]+(?:\\.[0-9][0-9])?)(?![\\d])";
                jsonString = Regex.Replace(jsonString, pattern, "FlatFee");
                return JsonConvert.DeserializeObject<List<T>>(jsonString);
            }
        }

        private string GetLocalCacheFilePath<T>()
        {
            return Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, $"{typeof(T).Name}s.json");
        }


        #endregion
    }
}
