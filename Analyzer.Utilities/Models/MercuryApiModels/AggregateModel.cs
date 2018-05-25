using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer.Utilities.Models.MercuryApiModels
{
    public class AggregateModel
    {
        public List<ResponseModel> ResponseModels { get; set; }
        public UrlModel UrlModel { get; set; }
    }
}
