using GoLah.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace GoLah.Services
{
    public class LtaDataRepository
    {
        private const string URI = @"http://datamall2.mytransport.sg/ltaodataservice/";
        private string key = @"4DZEmxtLQOmpRFW8vgqmTA==";
        private string accept = "application/json";

        private const string BUS_STOPS = "BusStops";
        private const string BUS_SERVICES = "BusServices";
        private const string BUS_ARRIVAL = "BusArrival?BusStopID={0}";

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

        public async Task<IEnumerable<BusStop>> GetBusStopsAsync()
        {
            var jsonString = await GetResponseStringAsync(string.Concat(URI, BUS_STOPS));
            return JsonConvert.DeserializeObject<OData<BusStop>>(jsonString)?.Value;
        }

        public async Task<IEnumerable<BusService>> GetBusServicesAsync()
        {
            var jsonString = await GetResponseStringAsync(string.Concat(URI, BUS_SERVICES));
            return JsonConvert.DeserializeObject<OData<BusService>>(jsonString)?.Value;
        }

        public async Task<IEnumerable<ArrivalBusService>> GetNextBusAsync(string busStopId)
        {
            var jsonString = await GetResponseStringAsync(string.Concat(URI, string.Format(BUS_ARRIVAL, busStopId)));
            return JsonConvert.DeserializeObject<BusArrivalOData>(jsonString)?.Services;
        }

        /* ToDo: Generic implementation
        public async Task<IEnumerable<ILtaData>> GetLtaDataAsync(Type ltaDataType)
        {
            if (ltaDataType == null || ltaDataType != typeof(ILtaData))
                return Enumerable.Empty<ILtaData>();
            var jsonString = await GetResponseStringAsync(string.Concat(URI, ltaDataType.FullName, "s"));
            return JsonConvert.DeserializeObject<OData<ILtaData>>(jsonString)?.Value;
        }
        */
    }
}
