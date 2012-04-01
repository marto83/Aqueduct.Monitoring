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
		static volatile bool _initialised;

		static ReadingPublisher()
		{
			Readings = new ConcurrentQueue<Reading>();
			Subscribers = new List<ReadingSubscriber>();
		}

		internal static IList<ReadingSubscriber> Subscribers { get; private set; }
		internal static ConcurrentQueue<Reading> Readings { get; private set; }

		public static void Start(int processInterval, bool enableTimer = true)
		{
			_timer = new Timer();
			_timer.Interval = processInterval;
			_timer.Elapsed += Timer_Elapsed;

			if (enableTimer)
				_timer.Start();

			_initialised = true;
		}

		public static void Stop()
		{
			_initialised = false;

			if (_timer == null)
				return;

			_timer.Stop();
			_timer.Elapsed -= Timer_Elapsed;
			_timer.Dispose();
		}

		public static void Subscribe(ReadingSubscriber subscriber)
		{
			Subscribers.Add(subscriber);
		}

		public static void AddReading(Reading reading)
		{
			Readings.Enqueue(reading);
		}

		internal static void Reset()
		{
			Readings = new ConcurrentQueue<Reading>();
			Subscribers.Clear();
		}

		internal static void Process()
		{
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
	}
}