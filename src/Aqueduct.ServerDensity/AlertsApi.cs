using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.ServerDensity
{
    public sealed class AlertsApi : IAlertsApi
    {
        private const string ModuleName = "alerts";
        private readonly ServerDensityApi _ApiBase;
        public AlertsApi(ServerDensityApi apiBase)
        {
            _ApiBase = apiBase;
        }

        public string GetLast()
        {
            return _ApiBase.CallUrl(ModuleName, "getLast");
        }
    }
}
