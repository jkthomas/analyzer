using Analyzer.Service.Parsers;
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
        private string _uri;

        static HttpRequestController()
        {
            _httpClient = new HttpClient();
        }
        public HttpRequestController(string uri)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            this._uri = uri;
        }

        public void AddContentTypeHeader(string contentType)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
        }

        public void AddAutorizationHeader(string key)
        {
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key);
            _httpClient.DefaultRequestHeaders.Add("x-api-key", key);
        }

        public async Task<JObject> Send()
        {
            var response = await _httpClient.GetAsync(_uri);
            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);
            return json;
        }
    }
}
