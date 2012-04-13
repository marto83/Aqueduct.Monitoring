using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Specialized;
using Aqueduct.Diagnostics;

namespace Aqueduct.ServerDensity
{
    public sealed class MetricsApi : IMetricsApi
    {
        readonly static ILogger Logger = AppLogger.GetNamedLogger(typeof(ServerDensityApi));

        private const string ModuleName = "metrics";
        private readonly ServerDensityApi _ApiBase;
        
        public MetricsApi(ServerDensityApi apiBase)
        {
            _ApiBase = apiBase;
        }

        public string UploadPluginData(string deviceId, MetricsPayload payload)
        {
            Logger.LogDebugMessage("Uploading plugin data for device " + deviceId);

            NameValueCollection postData = new NameValueCollection();
            postData["payload"] = JsonConvert.SerializeObject(payload);

            var extraParams = new NameValueCollection();
            extraParams["deviceId"] = deviceId;

            return _ApiBase.PostTo(ModuleName, "postback", extraParams, postData);
        }
    }
}
