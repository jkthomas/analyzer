using Analyzer.Service.Counter;
using Analyzer.Service.HttpClientService;
using Analyzer.Service.Parsers;
using Analyzer.Utilities.ApiFactory;
using Analyzer.Utilities.ApiFactory.Mercury;
using Analyzer.Utilities.Models;
using Analyzer.Utilities.Models.MercuryApiModels;
using Analyzer.Utilities.StaticContent;
using Analyzer.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private static AggregateModel _viewModel;

        //public string _userUrl = "https://trackchanges.postlight.com/building-awesome-cms-f034344d8ed";

        static MercuryApiController()
        {
            _viewModel = new AggregateModel()
            {
                ResponseModels = new List<ResponseModel>()
            };
        }

        public MercuryApiController()
        {
            this._parser = new JsonParser();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UrlModel urlModel)
        {
            _apiFactory = new MercuryApiFactory(ApiUris.MercuryApiUri + urlModel.ProvidedUrl, ContentTypes.Json, ApiNames.MercuryApiName, AuthorizationTypes.xKey);
            this._httpRequestController = new HttpRequestController(_apiFactory.GetApi());

            _parser.Parse("api.json");
            _httpRequestController.AddContentTypeHeader();
            _httpRequestController.AddAutorizationHeader(_parser.GetObject(ApiNames.MercuryApiName));
            var result = await _httpRequestController.Send();
            var response = JsonConvert.DeserializeObject<ResponseModel>(result);

            if (_viewModel.ResponseModels.Count == 5)
            {
                _viewModel.ResponseModels.RemoveAt(0);
                _viewModel.ResponseModels.Add(response);
            }
            else
            {
                _viewModel.ResponseModels.Add(response);
            }

            HtmlCounter counter = new HtmlCounter(_viewModel.ResponseModels.Last().content);
            _viewModel.ResponseModels.Last().TagsOccurrences = new Dictionary<string, int>();
            _viewModel.ResponseModels.Last().TagsOccurrences = counter.CountOccurrence();

            return View(_viewModel);
        }

        public IActionResult ShowContent()
        {

            return View("ShowContent", _viewModel);
        }

        //[ValidateInput(false)]
        [Produces("text/html")]
        public string ShowHtml(string itemUrl)
        {
            string htmlContent = _viewModel.ResponseModels.Where(url => url.Equals(itemUrl)).ToString();
            return htmlContent;
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
