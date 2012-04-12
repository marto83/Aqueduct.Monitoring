using System;
using System.Collections.Generic;
using System.Linq;
using Aqueduct.Diagnostics;

namespace Aqueduct.Monitoring.Subscribers
{
    public static class LoggingSubscriber
    {
        private readonly static ILogger Logger = AppLogger.GetNamedLogger(typeof(LoggingSubscriber));
        public static void Subscribe()
        {
            ReadingPublisher.Subscribe(new ReadingSubscriber(typeof(LoggingSubscriber).Name, ProcessStats));
        }

        private static void ProcessStats(IList<FeatureStatistics> stats)
        {
            foreach (var featureStat in stats)
            {
                Logger.LogInfoMessage(String.Format("------------------- Feature {0} in {1} at {2} ----------------------", featureStat.Name, featureStat.Group, featureStat.Timestamp)); 
                foreach (var reading in featureStat.Readings)
                {
                	Logger.LogInfoMessage(String.Format("ReadingName: {0}; Value: {1}", reading.Name, reading.GetValue()));
                }
                Logger.LogInfoMessage("---------------------------------------------------");
            }
        }
    }
}
