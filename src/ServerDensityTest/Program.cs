using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using Aqueduct.Diagnostics.Monitoring;
using Newtonsoft.Json;
using System.Web;

namespace ServerDensityTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var newDataPoint = new FeatureStats();
            newDataPoint.Name = "IndexController";
            newDataPoint.Readings.Add(new NumberReadingData(10) { Name = "Requests" });
            newDataPoint.Readings.Add(new NumberReadingData(1) { Name = "Errors" });
            List<FeatureStats> points = new List<FeatureStats> { newDataPoint };

            ServerDensityPayload payload = new ServerDensityPayload("WebsiteStats", "77a63a6e708acb1e7d88f86257b75783");

            payload.AddDataPoints(points);
            Api api = new Api();
            Console.Write(api.UploadData(payload.GetJson()));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            
        }
    }

    public class Api
    {
        string api_key = "5mhrqtjdaxar4pvvuvwekpch";
        string username = "aqueductnotifier";
        string password = "4qu3notifier";
        string accountUrl = "aqueduct.serverdensity.com";
        string apiUrl = "api.serverdensity.com/1.4/"; 

        private void Call(Action<WebClient> action)
        {
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(username, password);
                client.Headers["Content-type"] = "application/x-www-form-urlencoded";
                client.Headers["User-Agent"] = "Aqueduct stat uploader";

                action.Invoke(client);
            }
        }

        public string UploadData(string json)
        {
            var postValue = Encoding.UTF8.GetBytes("&payload=" + HttpUtility.UrlEncode(json));
            byte[] result = new byte[0];
            Call((client) =>
            {
                result = client.UploadData(BuildUrl("metrics", "postback") + "&deviceId=4f71c92d004cb602080000f8", "POST", postValue);
            });

            return Encoding.UTF8.GetString(result);
        }

        string BuildUrl(string module, string method)
        {
            return string.Format("https://{0}{1}/{2}?account={3}&apiKey={4}", apiUrl, module, method, accountUrl, api_key);
        }
    }

    public class ServerDensityPayload
    {
        private readonly string _appName;
        public string agentKey { get; set; }
        public Dictionary<string, object> plugins { get; private set; }

        public ServerDensityPayload(string appName, string key)
        {
            agentKey = key;
            _appName = appName;
            plugins = new Dictionary<string, object>();
        }

        public void AddDataPoints(IList<FeatureStats> datapoints)
        {
            Dictionary<string, object> stats = new Dictionary<string, object>();
            foreach (var point in datapoints)
            {
                foreach (var readingData in point.Readings)
                {
                    stats[point.Name + "_" + readingData.Name] = readingData.GetValue().ToString();
                }
            }
            plugins[_appName] = stats;
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
