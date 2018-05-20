using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analyzer.Web.ViewModels.MercuryApi
{
    public class ResponseViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public decimal Words { get; set; }
    }
}
