using System;
using System.Collections.Generic;
using System.Linq;
using Aqueduct.ServerDensity;
using Aqueduct.Diagnostics;

namespace Aqueduct.Monitoring.ServerDensity
{
    public class ServerDensitySubscriber
    {
        readonly static ILogger Logger = AppLogger.GetNamedLogger(typeof(ServerDensitySubscriber));
        private readonly string _AgentKey;
        public const string ServerDensityFeatureGroup = "serverdensity";
        private IServerDensityApi _api;
        private readonly string _deviceId;
        
        public ServerDensitySubscriber(string deviceId, string agentKey)
        {
            _AgentKey = agentKey;
            _deviceId = deviceId;
            _api = ServerDensityApi.Initialise();
        }

        public void Subscribe()
        {
            ReadingPublisher.Subscribe(new ReadingSubscriber("ServerDensity", ProcessStats));
        }

        private void ProcessStats(IList<FeatureStatistics> stats)
        {
            Logger.LogDebugMessage("Creating ServerDensity payload");
            var payload = new MetricsPayload() { AgentKey = _AgentKey };

            foreach (var featureStat in stats.Where(x => x.Group == ServerDensityFeatureGroup))
            {
                var plugin = new ServerDensityPlugin(featureStat.Name);
                foreach (var reading in featureStat.Readings)
                {
                	plugin.Add(reading.Name, reading.GetValue());
                }
                payload.AddPlugin(plugin);
            }

            Logger.LogDebugMessage(String.Format("Uploading {0} stats to ServerDensity", stats.Count));
            _api.Metrics.UploadPluginData(_deviceId, payload);
        }
    }
}
