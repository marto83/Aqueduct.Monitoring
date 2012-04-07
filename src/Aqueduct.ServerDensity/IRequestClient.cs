using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;

namespace Aqueduct.ServerDensity
{
    public interface IRequestClient
    {
        string Get(string url);
        string Post(string url, string postbody);
    }
}
