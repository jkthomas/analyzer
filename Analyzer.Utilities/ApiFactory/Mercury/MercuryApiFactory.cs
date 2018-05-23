using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer.Utilities.ApiFactory.Mercury
{
    public class MercuryApiFactory : ApiFactory
    {
        private string _uri { get; }          // = "https://mercury.postlight.com/parser?url=";
        private string _contentType { get; }  // = "application/json";
        private string _apiName { get; }      // = "mercury";
        private string _siteUrl { get; }

        public MercuryApiFactory(string uri, string contentType, string apiName, string siteUrl)
        {
            this._uri = uri;
            this._contentType = contentType;
            this._apiName = apiName;
            this._siteUrl = siteUrl;
        }

        public override Api GetApi()
        {
            return new MercuryApi(this._uri, this._contentType, this._apiName, this._siteUrl);
        }
    }
}
