using Analyzer.Service.Parsers;
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

        public void AddContentTypeHeader(/*string headerFieldName, string headerFieldValue*/)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void AddAutorizationHeader(string key)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key);
        }

        public async Task<string> Send()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_uri);
            return response.ToString();
        }
    }
}
