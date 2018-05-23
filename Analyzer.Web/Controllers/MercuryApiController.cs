using Analyzer.Service.HttpClientService;
using Analyzer.Service.Parsers;
using Analyzer.Utilities.StaticContent;
using Analyzer.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
            this._httpRequestController = new HttpRequestController(ApiUris.MercuryApiUri + "https://trackchanges.postlight.com/building-awesome-cms-f034344d8ed");
            this._parser = new JsonParser();
        }
        public async Task<IActionResult> Index()
        {
            _parser.Parse("api.json");
            _httpRequestController.AddContentTypeHeader("application/json");
            _httpRequestController.AddAutorizationHeader(_parser.GetObject(_apiName));
            JObject result = await _httpRequestController.Send();
            return Json(result);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
