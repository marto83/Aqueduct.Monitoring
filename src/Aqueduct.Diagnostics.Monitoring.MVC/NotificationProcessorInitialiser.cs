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
            ReadingPublisher.Start(60000);
        }

        public static void HandleError(Exception lastError)
        {
            Exception error = lastError.GetBaseException();
            if (error is HttpException && ((HttpException)error).ErrorCode == 404) return;
            
            var sensor = new ExceptionSensor();
            sensor.AddError(error);
        }

        public static void Subscribe(string name, Action<IList<FeatureStatistics>> dataAction)
        {
            var subscriber = new ReadingSubscriber(name, dataAction);
			ReadingPublisher.Subscribe(subscriber);
        }

        public static void Shutdown ()
        {
            ReadingPublisher.Stop();
        }
    }
}

