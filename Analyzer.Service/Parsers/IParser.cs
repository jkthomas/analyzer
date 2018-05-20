using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer.Service.Parsers
{
    public interface IParser<T>
    {
        void Parse(string filename);
        T GetObject(string keyword);
    }
}
