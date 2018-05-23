using Analyzer.Service.HttpClientService;
using Analyzer.Service.Parsers;
using Analyzer.Utilities.ApiFactory;
using Analyzer.Utilities.ApiFactory.Mercury;
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
        private ApiFactory _apiFactory;
        private HttpRequestController _httpRequestController;

        public string _userUrl = "https://trackchanges.postlight.com/building-awesome-cms-f034344d8ed";

        public MercuryApiController()
        {
            _apiFactory = new MercuryApiFactory(ApiUris.MercuryApiUri + _userUrl, ContentTypes.Json, ApiNames.MercuryApiName, AuthorizationTypes.xKey);
            this._httpRequestController = new HttpRequestController(_apiFactory.GetApi());
            this._parser = new JsonParser();
        }

        public async Task<IActionResult> Index()
        {
            _parser.Parse("api.json");
            _httpRequestController.AddContentTypeHeader();
            _httpRequestController.AddAutorizationHeader(_parser.GetObject(ApiNames.MercuryApiName));
            JObject result = await _httpRequestController.Send();
            return Json(result);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
