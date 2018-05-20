using Analyzer.Service.HttpClientService;
using Analyzer.Service.Parsers;
using Analyzer.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Analyzer.Web.Controllers
{
    public class MercuryApiController : Controller
    {
        private IParser<string> _parser;
        private HttpRequestController _httpRequestController;
        private string _uri = "https://mercury.postlight.com/parser?url=";
        private string _contentType = "application/json";
        private string _apiName = "mercury";

        public MercuryApiController()
        {
            this._httpRequestController = new HttpRequestController(this._uri + "https://trackchanges.postlight.com/building-awesome-cms-f034344d8ed");
            this._parser = new JsonParser();
        }
        public async Task<IActionResult> Index()
        {
            _parser.Parse("api.json");
            _httpRequestController.AddContentTypeHeader();
            //TODO: Fix authorization. Header doesn't match the pattern
            _httpRequestController.AddAutorizationHeader(_parser.GetObject(_apiName));
            string result = await _httpRequestController.Send();
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
