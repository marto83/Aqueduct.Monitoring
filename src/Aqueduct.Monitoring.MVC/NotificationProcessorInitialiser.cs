using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
using Aqueduct.Monitoring.Sensors;

namespace Aqueduct.Monitoring.MVC
{
    public class MVCNotificationProcessor
    {
        public static void Initialise(GlobalFilterCollection filters)
        {
            filters.Add(new NotificationFilter());
           ReadingPublisher.Start((int)TimeSpan.FromMinutes(1).TotalMilliseconds);
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

