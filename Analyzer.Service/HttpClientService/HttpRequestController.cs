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
            //Stops usage of proxy to speed up async connection
            HttpClientHandler httpHandler = new HttpClientHandler
            {
                Proxy = null,
                UseProxy = false
            };

            _httpClient = new HttpClient(httpHandler);
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
            _httpClient.DefaultRequestHeaders.Add(_api.AuthorizationType, key);
        }

        public async Task<string> Send()
        {
            var response = await _httpClient.GetAsync(_api.Uri);
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
