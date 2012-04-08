using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Aqueduct.ServerDensity
{
    public class MetricsPayload
    {
        [JsonProperty("agentKey")]
        public string AgentKey { get; set; }

        [JsonProperty("plugins")]
        public Dictionary<string, ServerDensityPlugin> Plugins { get; set; }

        /// <summary>
        /// Initializes a new instance of the etricsPayload class.
        /// </summary>
        public MetricsPayload()
        {
            Plugins = new Dictionary<string, ServerDensityPlugin>();
        }

        public void AddPlugin(ServerDensityPlugin plugin)
        {
            Plugins.Add(plugin.Name, plugin);
        }
    }
}
