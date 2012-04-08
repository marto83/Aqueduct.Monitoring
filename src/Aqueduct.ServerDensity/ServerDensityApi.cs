using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Collections.Specialized;

namespace Aqueduct.ServerDensity
{
    public class ServerDensityApi : IServerDensityApi
    {
        private const string UrlTemplate = "https://{0}/{1}/{2}/{3}?account={4}&apiKey={5}{6}";
        private string _apiKey;
        private NetworkCredential _credentials;
        private string _version;
        private string _account;
        private string _apiUrl;

        private IRequestClient RequestClient { get; set; }

        public static IServerDensityApi Initialise()
        {
            return Initialise(ServerDensitySettings.GetFromAppConfig());
        }

        public static IServerDensityApi Initialise(ServerDensitySettings settings)
        {
            return Initialise(settings, null);
        }

        internal static IServerDensityApi Initialise(ServerDensitySettings settings, IRequestClient requestClient)
        {
            if (string.IsNullOrEmpty(settings.ApiKey))
                throw new ArgumentException("Api key missing. Api key is required to initialise API");

            if (settings.Credentials == null || string.IsNullOrEmpty(settings.Credentials.UserName) || string.IsNullOrEmpty(settings.Credentials.Password))
                throw new ArgumentException("Credetials are required for the api calls to function correctly.");

            if (string.IsNullOrEmpty(settings.Account))
                throw new ArgumentException("Account missing. Account is required to initialise API. It will be in the form of [name].serverdensity.com");

            return new ServerDensityApi(settings) { RequestClient = requestClient ?? new RequestClient() };
        }

        internal string CallUrl(string module, string method)
        {
            return RequestClient.Get(string.Format(UrlTemplate, _apiUrl, _version, module, method, _account, _apiKey, ""));
        }

        public string PostTo(string module, string method, NameValueCollection extraQueryParams, NameValueCollection postData)
        {
            return RequestClient.Post(string.Format(UrlTemplate, _apiUrl, _version, module, method, _account, _apiKey, extraQueryParams.ToQueryString()), postData.ToQueryString());
        }

        public string Version { get { return _version; } }

        private ServerDensityApi(ServerDensitySettings settings)
        {
            _apiKey = settings.ApiKey;
            _credentials = settings.Credentials;
            _account = settings.Account;
            _apiUrl = settings.ApiUrl;
            _version = settings.Version;

            Alerts = new AlertsApi(this);
            Metrics = new MetricsApi(this);
        }

        public IAlertsApi Alerts { get; private set; }
        public IMetricsApi Metrics { get; private set; }
    }
}
