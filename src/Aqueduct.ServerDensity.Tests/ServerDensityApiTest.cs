using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Net;
using System.Collections.Specialized;
using Moq;

namespace Aqueduct.ServerDensity.Tests
{
   

    [TestFixture]
    public class ServerDensityApiTest : TestBase
    {
        [Test]
        public void Initilise_ReturnsIServerDensityApi()
        {
            var api = GetApi();

            Assert.That(api, Is.InstanceOf<IServerDensityApi>());
            Assert.That(api, Is.Not.Null);
        }

        [Test]
        public void Initilise_WhenApiKeyIsNotSpecified_ThrowsException()
        {
            var settings = new ServerDensitySettings("", "", null);
            Assert.Throws<ArgumentException>(() =>
            {
                ServerDensityApi.Initialise(settings);
            });
        }

        [Test]
        public void Initilise_WhenCredentialsNotSet_ThrowsException()
        {
            var settings = new ServerDensitySettings("", "key", null);
            Assert.Throws<ArgumentException>(() =>
                {
                    ServerDensityApi.Initialise(settings);
                });
        }

        [Test]
        public void Initialise_WhenAccountIsNotSet_ThrowsException()
        {
            var settings = new ServerDensitySettings("", "key", new NetworkCredential("u", "p"));
            Assert.Throws<ArgumentException>(() =>
            {
                ServerDensityApi.Initialise(settings);
            });
        }

        [Test]
        public void Version_Returns1point4()
        {
            var api = ServerDensityApi.Initialise(GetValidSettings());
            Assert.That(api.Version, Is.EqualTo("1.4"));
        }
    }

    [TestFixture]
    public class ServerDensityAlertsApiTests : TestBase
    {

        [Test]
        public void LastFive_ReturnsAResult()
        {
            var api = GetApi();

            Assert.That(api.Alerts.GetLast(), Is.Not.Null) ;
        }

        [Test]
        public void GetLast_MakesRequestToCorrectUrl()
        {
            var api = GetApi();
            
            var expectedUrl = GetExpectedUrl("alerts", "getLast");
            api.Alerts.GetLast();
            RequestClientMock.Verify(x => x.Get(expectedUrl));
        }
    }

    [TestFixture]
    public class ServerDensityMetricsApiTests : TestBase
    {
        [Test]
        public void Postback_CallsSendsaPostRequest()
        {
            var api = GetApi();

            var expectedUrl = GetExpectedUrl("metrics", "postback");
            api.Metrics.UploadPluginData("","", null);
            RequestClientMock.Verify(x => x.Post(expectedUrl, It.IsAny<string>()));
        }


    }
}
