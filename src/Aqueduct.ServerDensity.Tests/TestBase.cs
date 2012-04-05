using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Net;
using Moq;

namespace Aqueduct.ServerDensity.Tests
{
    public abstract class TestBase
    {
        private const string UrlTemplate = "https://{0}/{1}/{2}/{3}?account={4}&apiKey={5}{6}";
        protected Mock<IRequestClient> RequestClientMock;

        [SetUp]
        public void Setup()
        {
            RequestClientMock = new Mock<IRequestClient>();
        }


        protected string GetExpectedUrl(string module, string method)
        {
            var setting = GetValidSettings();
            return String.Format(UrlTemplate, setting.ApiUrl, setting.Version, module, method, setting.Account, setting.ApiKey, "");
        }

        protected ServerDensitySettings GetValidSettings()
        {
            return new ServerDensitySettings("account", "key", new NetworkCredential("user", "pass"));
        }

        protected IServerDensityApi GetApi()
        {
            return ServerDensityApi.Initialise(GetValidSettings(), RequestClientMock.Object);
        }


    }
}
