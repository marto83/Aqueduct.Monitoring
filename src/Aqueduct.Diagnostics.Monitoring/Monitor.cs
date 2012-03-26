using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Timers;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class Monitor
    {
        private static Timer Timer;
        private static ILogger Logger = AppLogger.GetNamedLogger(typeof(Monitor));
        internal static IList<MonitorSubscriber> Subscribers { get; private set; }
        internal static ConcurrentQueue<Reading> Readings { get; private set; }
        private static volatile bool _initialised = false;

        static Monitor()
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

        public static void Process()
        {
            var dataPoints = new List<DataPoint>();
            Reading reading;
            while (Readings.TryDequeue(out reading))
            {
                var dataPoint = dataPoints.FirstOrDefault(dp => dp.Name == reading.Name);
                if(dataPoint == null)
                {
                    dataPoint = new DataPoint() { Name = reading.Name };
                    dataPoints.Add(dataPoint);
                }

                dataPoint.Data += reading.Value;
            }

            NotifySubscribers(dataPoints);
        }

        private static void NotifySubscribers(IList<DataPoint> dataPoints)
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

        public static void Reset()
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

