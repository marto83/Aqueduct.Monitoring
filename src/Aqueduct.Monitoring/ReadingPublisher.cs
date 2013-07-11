using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Aqueduct.Monitoring.Readings;
using NLog;

namespace Aqueduct.Monitoring
{
	public static class ReadingPublisher
	{
		static Timer _timer;
		static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		static bool _initialised;
		static readonly object InitialisationLock = new object();
		static readonly object AddSubscriberLock = new object();
		static readonly object PublishReadingLock = new object();

		static ReadingPublisher()
		{
			Readings = new ConcurrentQueue<Reading>();
			Subscribers = new List<ReadingSubscriber>();
		}

		internal static IList<ReadingSubscriber> Subscribers { get; private set; }
		internal static ConcurrentQueue<Reading> Readings { get; private set; }

		public static void Start(int processInterval, bool enableTimer = true)
		{
            Logger.Debug("Starting publisher");
			if (_initialised)
				return;

			lock (InitialisationLock)
			{
				if (_initialised)
					return;
                Logger.Debug("Initialising timer");
				_timer = new Timer();
				_timer.Interval = processInterval;
				_timer.Elapsed += Timer_Elapsed;

				if (enableTimer)
					_timer.Start();

				_initialised = true;
			}
		}

		public static void Stop()
		{
            Logger.Debug("Stopping timer and disposing of the timer");
			if (_initialised == false)
				return;

			lock (InitialisationLock)
			{
				if (_initialised == false)
					return;

				if (_timer == null)
					return;

				_timer.Stop();
				_timer.Elapsed -= Timer_Elapsed;
				_timer.Dispose();

				_initialised = false;
			}
		}

		public static void Subscribe(ReadingSubscriber subscriber)
		{
            Logger.Debug("Adding subscriber " + subscriber.Name);

			lock (AddSubscriberLock)
			{
				Subscribers.Add(subscriber);
			}
		}

		public static void PublishReading(Reading reading)
		{
            Logger.Debug("Enqueuing reding " + GetReadingInfo(reading));
			lock (PublishReadingLock)
			{
				Readings.Enqueue(reading);
			}
		}

        private static string GetReadingInfo(Reading reading)
        {
            if (reading != null)
                return String.Format("FeatureName: {0}, group: {1}, reading; {2}, value: {3}", reading.FeatureName, reading.FeatureGroup, reading.Data.Name, reading.Data.GetValue());
            return "Null reading";
        }

        internal static void Reset()
		{
            Logger.Debug("Resetting publisner: clearing queue and removing all subscribers");
			lock (PublishReadingLock)
			{
				Readings = new ConcurrentQueue<Reading>();
			}

			lock (AddSubscriberLock)
			{
				Subscribers.Clear();
			}
		}

		internal static void Process()
		{
            Logger.Debug("Processing readings");
			var dataPoints = new List<FeatureStatistics>();
			Reading reading;
			while (Readings.TryDequeue(out reading))
			{
				ProcessReading(dataPoints, reading);
			}

			NotifySubscribers(dataPoints);
		}

		static void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Process();
		}

		static void ProcessReading(ICollection<FeatureStatistics> dataPoints, Reading reading)
		{
			var dataPoint = dataPoints.FirstOrDefault(data => data.Name == reading.FeatureName && data.Group == reading.FeatureGroup);
			if (dataPoint == null)
			{
				dataPoint = new FeatureStatistics { Name = reading.FeatureName, Group = reading.FeatureGroup };
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

		static void NotifySubscribers(IList<FeatureStatistics> dataPoints)
		{
            Logger.Debug("Notifying Subscribers");
			if (_initialised == false)
				return;

			lock (InitialisationLock)
			{
				if (_initialised == false)
					return;

				foreach (var subscriber in Subscribers)
				{
					try
					{
                        Logger.Debug("Notifying subsriber: " + subscriber.Name);
						subscriber.ProcessStatistics(dataPoints);
					}
					catch (Exception ex)
					{
						Logger.ErrorException(String.Format("Error while executing subscriber: {0}", subscriber.Name), ex);
					}
				}
			}
		}
	}
}