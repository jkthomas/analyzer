using Analyzer.Service.Parsers;
using Analyzer.Utilities.ApiFactory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Service.HttpClientService
{
    public class HttpRequestController
    {
        private static readonly HttpClient _httpClient;
        private Api _api;

        static HttpRequestController()
        {
            _httpClient = new HttpClient();
        }
        public HttpRequestController(Api api)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            this._api = api;
        }

        public void AddContentTypeHeader()
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_api.ContentType));
        }

        public void AddAutorizationHeader(string key)
        {
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key);
            _httpClient.DefaultRequestHeaders.Add(_api.AuthorizationType, key);
        }

        public async Task<JObject> Send()
        {
            var response = await _httpClient.GetAsync(_api.Uri);
            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);
            return json;
        }
    }
}
