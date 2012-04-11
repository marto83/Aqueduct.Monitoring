using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Aqueduct.Diagnostics.Monitoring.Readings;

namespace Aqueduct.Diagnostics.Monitoring
{
	public static class ReadingPublisher
	{
		static Timer _timer;
		static readonly ILogger Logger = AppLogger.GetNamedLogger(typeof(ReadingPublisher));
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
            Logger.LogDebugMessage("Starting publisher");
			if (_initialised)
				return;

			lock (InitialisationLock)
			{
				if (_initialised)
					return;
                Logger.LogDebugMessage("Initialising timer");
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
            Logger.LogDebugMessage("Stopping timer and disposing of the timer");
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
            Logger.LogDebugMessage("Adding subscriber " + subscriber.Name);

			lock (AddSubscriberLock)
			{
				Subscribers.Add(subscriber);
			}
		}

		public static void PublishReading(Reading reading)
		{
            Logger.LogDebugMessage("Enqueuing reding " + GetReadingInfo(reading));
			lock (PublishReadingLock)
			{
				Readings.Enqueue(reading);
			}
		}

        private static string GetReadingInfo(Reading reading)
        {
            if (reading != null)
                return String.Format("FeatureName: {0}, group: {1}, reading; {2}", reading.FeatureName, reading.FeatureGroup, reading.Data.Name);
            return "Null reading";
        }

        internal static void Reset()
		{
            Logger.LogDebugMessage("Resetting publisner: clearing queue and removing all subscribers");
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
            Logger.LogDebugMessage("Processing readings");
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
			var dataPoint = dataPoints.FirstOrDefault(data => data.Name == reading.FeatureName);
			if (dataPoint == null)
			{
				dataPoint = new FeatureStatistics { Name = reading.FeatureName };
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
            Logger.LogDebugMessage("Notifying Subscribers");
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
                        Logger.LogDebugMessage("Notifying subsriber: " + subscriber.Name);
						subscriber.ProcessStatistics(dataPoints);
					}
					catch (Exception ex)
					{
						Logger.LogError(String.Format("Error while executing subscriber: {0}", subscriber.Name), ex);
					}
				}
			}
		}
	}
}