using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.ServerDensity
{
    public interface IServerDensityApi
    {
        IMetricsApi Metrics { get; }
        IAlertsApi Alerts { get; }
        string Version { get; }
    }
}
