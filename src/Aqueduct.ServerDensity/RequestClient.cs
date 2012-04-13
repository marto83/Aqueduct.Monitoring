using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Aqueduct.Diagnostics;

namespace Aqueduct.ServerDensity
{
    public class RequestClient : IRequestClient
    {
        readonly static ILogger Logger = AppLogger.GetNamedLogger(typeof(ServerDensityApi));

        private readonly string _username;
        private readonly string _password;
        public RequestClient(string username, string password)
        {
            _password = password;
            _username = username;
        }

        /// <summary>
        /// Initializes a new instance of the RequestClient class.
        /// </summary>
        public RequestClient()
        {
            
        }

        private WebClient Create()
        {
            WebClient client = new WebClient() { Credentials = new NetworkCredential(_username, _password) };
            client.Headers["User-Agent"] = "Aqueduct stat uploader";
            return client;
        }

        public string Get(string url)
        {
            Logger.LogDebugMessage("Sending Request to url: " + url);

            using (WebClient client = Create())
            {
                return client.DownloadString(url);
            }
        }

        public string Post(string url, string postBody)
        {
            Logger.LogDebugMessage("Sending Request to url: " + url);
            using (WebClient client = Create())
            {
                client.Headers["Content-type"] = "application/x-www-form-urlencoded";

                var postValue = Encoding.UTF8.GetBytes(postBody);
                byte[] result = new byte[0];
                result = client.UploadData(url, "POST", postValue);

                return Encoding.UTF8.GetString(result);
            }
        }
    }
}
