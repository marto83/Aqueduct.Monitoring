using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aqueduct.ServerDensity;

namespace Aqueduct.Diagnostics.Monitoring.Subscribers
{
    public class ServerDensitySubscriber
    {
        private IServerDensityApi _api;
        private readonly string _deviceId;
        /// <summary>
        /// Initializes a new instance of the ServerDensitySubscriber class.
        /// </summary>
        public ServerDensitySubscriber(string deviceId)
        {
            _deviceId = deviceId;
            _api = ServerDensityApi.Initialise();
        }

        public void Subscribe()
        {
            ReadingPublisher.Subscribe(new ReadingSubscriber("ServerDensity", ProcessStats));
        }

        private void ProcessStats(IList<FeatureStatistics> stats)
        {
            _api.Metrics.UploadPluginData(_deviceId, null);
        }
    }
}
