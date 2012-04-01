using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
using Aqueduct.Diagnostics.Monitoring.Sensors;

namespace Aqueduct.Diagnostics.Monitoring.MVC
{
    public class MVCNotificationProcessor
    {
        public static void Initialise(GlobalFilterCollection filters)
        {
            filters.Add(new NotificationFilter());
            NotificationProcessor.Initialise(60000);
        }

        public static void HandleError(Exception lastError)
        {
            Exception error = lastError.GetBaseException();
            if (error is HttpException && ((HttpException)error).ErrorCode == 404) return;
            
            var sensor = new ExceptionSensor();
            sensor.AddError(error);
        }

        public static void Subscribe(string name, Action<IList<FeatureStats>> dataAction)
        {
            var subscriber = new MonitorSubscriber(name, dataAction);
            NotificationProcessor.Subscribe(subscriber);
        }

        public static void Shutdown ()
        {
            NotificationProcessor.Shutdown();
        }
    }
}

