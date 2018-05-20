using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Analyzer.Service.Parsers
{
    public class JsonParser : IParser<string>
    {
        public class Item
        {
            public string mercury { get; set; }
        }
        public Item OneItem { get; set; }
        

        public string GetObject(string keyword)
        {
            //string value = this.Items.Find(key => key.Equals(keyword)).Value;
            return this.OneItem.mercury;
        }

        public void Parse(string filename)
        {
            using (StreamReader r = new StreamReader(filename))
            {
                string json = r.ReadToEnd();
                this.OneItem = JsonConvert.DeserializeObject<Item>(json);
            }
        }
    }
}
