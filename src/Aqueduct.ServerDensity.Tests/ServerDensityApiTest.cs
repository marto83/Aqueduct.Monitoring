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
    public interface IServerDensityApi
    {
        IAlertsApi Alerts { get; }
        string Version { get; }
    }

    public class AlertsApi : IAlertsApi
    {
        public AlertsApi()
        {

        }

        public string GetLast()
        {
            return "something";
        }
    }
    public interface IAlertsApi
    {
        string GetLast();
    }

    public interface IRequestClient
    {
        string Get(string url);
        string Post(string url, NameValueCollection values);
    }

    public class RequestClient : IRequestClient
    {
        public RequestClient()
        {
            
        }

        public string Get(string url)
        {
            throw new NotImplementedException();
        }

        public string Post(string url, NameValueCollection values)
        {
            throw new NotImplementedException();
        }
    }
    public class ServerDensityApi : IServerDensityApi
    {
        private string _apiKey;
        private NetworkCredential _credentials;
        private string _version;
        private string _account;
        private string _apiUrl;

        private IRequestClient RequestClient { get; set; } 

        public static IServerDensityApi Initialise(ServerDensitySettings settings)
        {
            return Initialise(settings, null);
        }

        internal static IServerDensityApi Initialise(ServerDensitySettings settings, IRequestClient requestClient)
        {
            if(string.IsNullOrEmpty(settings.ApiKey))
                throw new ArgumentException("Api key missing. Api key is required to initialise API");
            
            if(settings.Credentials == null || string.IsNullOrEmpty(settings.Credentials.UserName) || string.IsNullOrEmpty(settings.Credentials.Password))
                throw new ArgumentException("Credetials are required for the api calls to function correctly.");

            if (string.IsNullOrEmpty(settings.Account))
                throw new ArgumentException("Account missing. Account is required to initialise API. It will be in the form of [name].serverdensity.com");

            return new ServerDensityApi(settings) { RequestClient = requestClient ?? new RequestClient() };
        }

        public string Version { get { return _version; } }

        private ServerDensityApi(ServerDensitySettings settings)
        {
            _apiKey = settings.ApiKey;
            _credentials = settings.Credentials;
            _account = settings.Account;
            _apiUrl = settings.ApiUrl;
            _version = settings.Version;

            Alerts = new AlertsApi();
        }

        public IAlertsApi Alerts { get; private set; }
    }

    public class ServerDensitySettings
    {
        public string Account { get; set; }
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
        public string Version { get; set; }
        public NetworkCredential Credentials { get; set; }

        /// <summary>
        /// Initializes a new instance of the ServerDensitySettings class.
        /// </summary>
        public ServerDensitySettings(string account, string apiKey, NetworkCredential credentials)
        {
            Account = account;
            ApiKey = apiKey;
            Credentials = credentials;
            ApiUrl = "api.serverdensity.com";
            Version = "1.4";
        }
    }

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
            
            var expectedUrl = GetExpectedUrl("alerts", "getlast");
            api.Alerts.GetLast();
            RequestClientMock.Verify(x => x.Get(expectedUrl));

        }
    }
}
