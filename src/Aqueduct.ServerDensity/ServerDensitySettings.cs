using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Configuration;

namespace Aqueduct.ServerDensity
{
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

        private ServerDensitySettings()
        {

        }

        public static ServerDensitySettings GetFromAppConfig()
        {
            var settings = new ServerDensitySettings();
            settings.Account = ConfigurationManager.AppSettings["ServerDensity.Account"];
            Validate("ServerDensity.Account", settings.Account);
            settings.ApiKey = ConfigurationManager.AppSettings["ServerDensity.ApiKey"];
            Validate("ServerDensity.ApiKey", settings.ApiKey);
            var username = ConfigurationManager.AppSettings["ServerDensity.Username"];
            var password = ConfigurationManager.AppSettings["ServerDensity.Password"];
            Validate("ServerDensity.Username", username);
            Validate("ServerDensity.Password", password);
            settings.Credentials = new NetworkCredential(username, password);
            var version = ConfigurationManager.AppSettings["ServerDensity.Version"] ?? "1.4";
            settings.Version = version;
            var apiUrl = ConfigurationManager.AppSettings["ServerDensity.ApiUrl"] ?? "api.serverdensity.com";
            settings.ApiUrl = apiUrl;
            return settings;

        }
private static void Validate(string name, string setting)
{
    if(string.IsNullOrWhiteSpace(setting))
    {
        throw new ConfigurationErrorsException(string.Format("Missing setting for ({0})", name) );
    }
}
    }
}
