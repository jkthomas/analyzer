using Analyzer.Utilities.StaticContent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer.Service.Counter
{
    public class HtmlCounter
    {
        private string _html;

        public HtmlCounter(string html)
        {
            this._html = html;
        }

        public Dictionary<string, int> CountOccurrence()
        {
            Dictionary<string, int> tagsOccurrence = new Dictionary<string, int>();
            foreach (string tag in HtmlTags.Tags)
            {
                tagsOccurrence.Add(tag, this.CountTags("<" + tag + " "));
            }
            return tagsOccurrence;
        }

        public int CountTags(string tag)
        {
            int count = 0;
            int i = 0;
            while ((i = this._html.IndexOf(tag, i)) != -1)
            {
                i += tag.Length;
                count++;
            }
            return count;
        }
    }
}
