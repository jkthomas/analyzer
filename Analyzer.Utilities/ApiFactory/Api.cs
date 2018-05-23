using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer.Utilities.ApiFactory
{
    public abstract class Api
    {
        public abstract string Uri { get; }
        public abstract string ContentType { get; }
        public abstract string ApiName { get; }
    }
}
