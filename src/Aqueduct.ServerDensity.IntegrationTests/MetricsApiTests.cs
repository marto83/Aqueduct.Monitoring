using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace Aqueduct.ServerDensity.IntegrationTests
{
    [TestFixture]
    public class MetricsApiTests
    {
        [Test]
        public void Payload_WithCorrectCredentialsFromConfig_ReturnSuccess()
        {
            IServerDensityApi api = ServerDensityApi.Initialise();

            var result = api.Metrics.UploadPluginData("4f71c92d004cb602080000f8", GetPayLoad());

            Assert.That(IsSuccess(result), "Failure: " + result);
        }

        private MetricsPayload GetPayLoad()
        {
            return new MetricsPayload
            {
                AgentKey = "77a63a6e708acb1e7d88f86257b75783",
                Plugins = new Dictionary<string, ServerDensityPlugin>  { 
                { "test", new ServerDensityPlugin("Test") }
            }
            };
        }

        private bool IsSuccess(string response)
        {
            var jResponse = JObject.Parse(response);
            return jResponse["status"].ToString() == "1";
        }
    }
}
