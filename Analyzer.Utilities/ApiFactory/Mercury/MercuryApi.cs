using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer.Utilities.ApiFactory.Mercury
{
    public class MercuryApi : Api
    {
        public override string Uri { get; }
        public override string ContentType { get; }
        public override string ApiName { get; }
        public override string AuthorizationType { get; }

        public MercuryApi(string uri, string contentType, string apiName, string authorizationType)
        {
            this.Uri = uri;
            this.ContentType = contentType;
            this.ApiName = apiName;
            this.AuthorizationType = authorizationType;
        }
    }
}
