using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.ServerDensity
{
    public interface IMetricsApi
    {
        string UploadPluginData(string deviceId, MetricsPayload payload);
    }
}
