using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Timers;

namespace Aqueduct.Diagnostics.Monitoring
{
    public static class NotificationProcessor
    {
        private static Timer Timer;
        private static ILogger Logger = AppLogger.GetNamedLogger(typeof(NotificationProcessor));
        internal static IList<MonitorSubscriber> Subscribers { get; private set; }
        internal static ConcurrentQueue<Reading> Readings { get; private set; }
        private static volatile bool _initialised = false;

        static NotificationProcessor()
        {
            Readings = new ConcurrentQueue<Reading>();
            Subscribers = new List<MonitorSubscriber>();
        }

        public static void Initialise(int processInterval, bool enableTimer = true)
        {
            Timer = new Timer();
            Timer.Interval = processInterval;
            Timer.Elapsed += Timer_Elapsed;
            if(enableTimer)
                Timer.Start();
            _initialised = true;
        }

        static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Process();
        }

        public static void Shutdown()
        {
            _initialised = false;
            if (Timer != null)
            {
                Timer.Stop();
                Timer.Elapsed -= Timer_Elapsed;
                Timer.Dispose();
            }
        }

        public static void Add(Reading reconding)
        {
            Readings.Enqueue(reconding);
        }

        internal static void Process()
        {
            var dataPoints = new List<FeatureStats>();
            Reading reading;
            while (Readings.TryDequeue(out reading))
            {
                ProcessReading(dataPoints, reading);
            }

            NotifySubscribers(dataPoints);
        }

        private static void ProcessReading(List<FeatureStats> dataPoints, Reading reading)
        {
            var dataPoint = dataPoints.FirstOrDefault(data => data.Name == reading.FeatureName);
            if (dataPoint == null)
            {
                dataPoint = new FeatureStats() { Name = reading.FeatureName };
                dataPoint.Readings.Add(reading.Data);
                dataPoints.Add(dataPoint);
            }
            else
            {
                var readingData = dataPoint.Readings.FirstOrDefault(rd => rd.Name == reading.Data.Name);
                if (readingData == null)
                    dataPoint.Readings.Add(reading.Data);
                else
                    readingData.Aggregate(reading.Data);
            }
        }
        private static void NotifySubscribers(IList<FeatureStats> dataPoints)
        {
            if (_initialised == false) return;

            foreach (var subscriber in Subscribers)
            {
                try
                {
                    subscriber.Action(dataPoints);
                }
                catch (Exception ex)
                {
                    Logger.LogError(String.Format("Error while executing subscriber: {0}", subscriber.Name), ex);
                }
            }
        }

        internal static void Reset()
        {
            Readings = new ConcurrentQueue<Reading>();
            Subscribers.Clear();
        }

        public static void Subscribe(MonitorSubscriber subscriber)
        {
            Subscribers.Add(subscriber);
        }
    }
}

