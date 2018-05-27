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
            if (urlModel.ProvidedUrl == null)
            {
                return View(_viewModel);
            }
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

        public IActionResult ShowHtml(string itemUrl)
        {
            ResponseModel responseModel = _viewModel.ResponseModels.Where(obj => obj.url.Equals(itemUrl)).First();
            string htmlContent = responseModel.content;
            return new ContentResult()
            {
                Content = htmlContent,
                ContentType = "text/html"
            };
        }

        public IActionResult ShowChart(string itemUrl)
        {
            TagStatisticViewModel statisticViewModel = new TagStatisticViewModel();
            ResponseModel responseModel = _viewModel.ResponseModels.Where(obj => obj.url.Equals(itemUrl)).First();
            var tags = responseModel.TagsOccurrences.Where(pair => pair.Value != 0).ToDictionary(k => k.Key, v => v.Value).ToList();
            tags.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            statisticViewModel.TagsCount = tags.Sum(element => element.Value);
            try
            {
                foreach (var tag in tags.Skip(0).Take(10))
                {
                    statisticViewModel.TopTags.Add(tag);
                }

                foreach (var tag in tags.Skip(10))
                {
                    statisticViewModel.RestTags.Add(tag);
                }
            }
            catch (Exception e)
            {
                if (statisticViewModel.TopTags.Count == 0)
                {
                    statisticViewModel.TopTags.Add(new KeyValuePair<string, int>("No tags to show", 0));
                    statisticViewModel.RestTags.Add(new KeyValuePair<string, int>("No tags to show", 0));
                }
            }
            finally
            {
                if(statisticViewModel.RestTags.Count == 0)
                {
                    statisticViewModel.RestTags.Add(new KeyValuePair<string, int>("No tags to show", 0));
                }
            }
            return View(statisticViewModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
