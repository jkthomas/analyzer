using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analyzer.Web.ViewModels
{
    public class TagStatisticViewModel
    {
        public TagStatisticViewModel()
        {
            this.TopTags = new List<KeyValuePair<string, int>>();
            this.RestTags = new List<KeyValuePair<string, int>>();
        }
        public List<KeyValuePair<string, int>> TopTags { get; set; }
        public List<KeyValuePair<string, int>> RestTags { get; set; }
        public double TagsCount { get; set; }
    }
}
