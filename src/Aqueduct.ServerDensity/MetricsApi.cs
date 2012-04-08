using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace Aqueduct.ServerDensity
{
    public sealed class MetricsApi : IMetricsApi
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

        public string UploadPluginData(string deviceId, MetricsPayload payload)
        {
            NameValueCollection postData = new NameValueCollection();
            postData["payload"] = JsonConvert.SerializeObject(payload);

            return _ApiBase.PostTo(ModuleName, "postback", null, postData);
        }
    }

}
