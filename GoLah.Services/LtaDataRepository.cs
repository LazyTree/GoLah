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
    public class LtaDataRepositoryBase<T> where T : LtaData, new()
    {
        #region Fields

        protected const string URI = @"http://datamall2.mytransport.sg/ltaodataservice/";
        protected const string key = @"4DZEmxtLQOmpRFW8vgqmTA==";
        protected const string accept = "application/json";

        protected static List<T> _cachedItems = new List<T>();

        #endregion

        #region Methods

        /// <summary>
        /// Get Lta data from REST API.
        /// </summary>
        /// <param name="refresh">True to get fresh result online. False to get result from local file cache or RAM cache.</param>
        /// <param name="query">Query string that need to pass to the request URL.</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> QueryAsync(bool refresh = false, string query = "")
        {
            var jsonString = await GetResponseStringAsync(string.Concat(URI, string.Format(new T().ServiceUrl, query))).ConfigureAwait(false);
            _cachedItems = JsonConvert.DeserializeObject<OData<T>>(jsonString)?.Service;
            return _cachedItems;
        }

        /// <summary>
        /// Get the Json response from LTA datamall REST API.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task<string> GetResponseStringAsync(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            var headers = new WebHeaderCollection();
            headers["AccountKey"] = key;
            headers["Accept"] = accept;
            httpWebRequest.Headers = headers;
            httpWebRequest.Method = "GET";

            var response = await httpWebRequest.GetResponseAsync().ConfigureAwait(false);
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }

        #endregion
    }

    public class LtaDataRepository<T> : LtaDataRepositoryBase<T> where T : LtaData, new()
    {
        #region Fields

        public const string PAGING_SKIP = @"?$skip={0}";
        public const int PAGE_SIZE = 50;

        #endregion

        #region Methods

        /// <summary>
        /// Get Lta paged data from REST API.
        /// </summary>
        /// <param name="refresh">True to get fresh result online. False to get result from local file cache or RAM cache.</param>
        /// <param name="query">Query string that need to pass to the request URL.</param>
        /// <returns></returns>
        public override async Task<IEnumerable<T>> QueryAsync(bool refresh = false, string query = "")
        {
            if (!refresh)
            {
                if (_cachedItems.Any())
                {
                    return _cachedItems;
                }
                else
                {
                    _cachedItems = await LoadCacheAsync<T>().ConfigureAwait(false);
                    if (_cachedItems.Any())
                    {
                        return _cachedItems;
                    }
                }
            }

            int page = 0;
            IEnumerable<T> result;
            _cachedItems.Clear();
            do
            {
                result = await QueryByPageAsync(page).ConfigureAwait(false);
                _cachedItems.AddRange(result.ToList());
                page += PAGE_SIZE;
            }
            while (result.Count() == PAGE_SIZE);

            await SaveCacheAsync(_cachedItems).ConfigureAwait(false);

            return _cachedItems;
        }

        /// <summary>
        /// Get the Lta data array by page (50 records per page)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private async Task<IEnumerable<T>> QueryByPageAsync(int page)
        {
            var jsonString = await GetResponseStringAsync(string.Concat(
                    URI, 
                    new T().ServiceUrl, 
                    page == 0 ? string.Empty : string.Format(PAGING_SKIP, page)
                    )).ConfigureAwait(false);
            
            // ugly workaround for lousy lta data
            if (typeof(T) == typeof(BusRoute))
            {
                var pattern = "(FLAT FARE \\$[0-9]+(?:\\.[0-9][0-9])?)(?![\\d])";
                jsonString = Regex.Replace(jsonString, pattern, "FlatFee");
            }

            return JsonConvert.DeserializeObject<OData<T>>(jsonString)?.Value;
        }

        /// <summary>
        /// Get the single 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(Func<T, bool> predicate)
        {
            var result = await QueryAsync().ConfigureAwait(false);
            return result.SingleOrDefault(predicate);
        }

        /// <summary>
        /// Save data array to the local cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_cachedItems"></param>
        /// <returns></returns>
        private async Task SaveCacheAsync<T>(List<T> _cachedItems)
        {
            var filePath = GetLocalCacheFilePath<T>();
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            using (var streamWriter = new StreamWriter(stream))
            {
                await streamWriter.WriteAsync(JsonConvert.SerializeObject(_cachedItems));
            }
        }

        /// <summary>
        /// Load the data array from the local cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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
                return JsonConvert.DeserializeObject<List<T>>(jsonString);
            }
        }

        /// <summary>
        /// Get local cache file by the naming convention: {TypeName}s.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private string GetLocalCacheFilePath<T>()
        {
            return Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, $"{typeof(T).Name}s.json");
        }

        #endregion
    }
}
