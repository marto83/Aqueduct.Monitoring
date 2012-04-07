using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Aqueduct.ServerDensity
{
    public class MetricsApi : IMetricsApi
    {
        private const string ModuleName = "metrics";
        private readonly ServerDensityApi _ApiBase;
        /// <summary>
        /// Initializes a new instance of the MetricsApi class.
        /// </summary>
        public MetricsApi(ServerDensityApi apiBase)
        {
            _ApiBase = apiBase;
        }

        public string UploadPluginData(string deviceId, string agentKey, Dictionary<string, object> plugins)
        {
            return _ApiBase.PostTo(ModuleName, "postback", "&payload=" + HttpUtility.UrlEncode(GetPluginsJson(agentKey, plugins)));
        }

        private string GetPluginsJson(string agentKey, Dictionary<string, object> plugins)
        {
            return JsonConvert.SerializeObject(new { agentKey = agentKey, plugins = plugins });
        }
    }
}
