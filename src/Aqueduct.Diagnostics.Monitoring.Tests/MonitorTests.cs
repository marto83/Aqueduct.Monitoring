using System;
using System.Collections.Generic;
using System.Linq;
using Aqueduct.Diagnostics.Monitoring.Readings;
using NUnit.Framework;
using System.Threading;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    [TestFixture]
    public class MonitorTests : MonitorTestBase
    {
        [Test]
        public void Add_AddsReadingToMonitor()
        {
            var reading = GetReading();
            ReadingPublisher.PublishReading(reading);

            Assert.AreEqual(1, ReadingPublisher.Readings.Count);
        }

        [Test]
        public void Reset_ClearsAllMonitorReadings()
        {
            var reading = GetReading();
            ReadingPublisher.PublishReading(reading);

            ReadingPublisher.Reset();

            Assert.That(ReadingPublisher.Readings.Count, Is.EqualTo(0));
        }

        [Test]
        public void Process_ClearsReadingsFromReadingsQueue()
        {
            ReadingPublisher.PublishReading(GetReading());
            ReadingPublisher.PublishReading(GetReading());

            ReadingPublisher.Process();

            Assert.That(ReadingPublisher.Readings.Count, Is.EqualTo(0));
        }

        [Test]
        public void Subscribe_RegistersSubscribesToTheMonitor()
        {
			ReadingPublisher.Subscribe(GetSubscriber((dataPoints) => Console.Write("Subscribed")));

            Assert.That(ReadingPublisher.Subscribers.Count, Is.EqualTo(1));
        }

        [Test]
        public void Reset_ClearsAllSubscribers()
        {
			ReadingPublisher.Subscribe(GetSubscriber((dataPoints) => Console.Write("Subscriber")));

            Assert.That(ReadingPublisher.Subscribers, Is.Not.Empty);

            ReadingPublisher.Reset();
            Assert.That(ReadingPublisher.Subscribers, Is.Empty);
        }

        [Test]
        public void Process_CallsAllSubsribers()
        {
            bool sub1called = false;
            bool sub2called = false;
			ReadingPublisher.Subscribe(GetSubscriber((dataPoints) => sub1called = true));
			ReadingPublisher.Subscribe(GetSubscriber((dataPoints) => sub2called = true));

            ReadingPublisher.Process();

            Assert.That(sub1called, Is.True);
            Assert.That(sub2called, Is.True);
        }

        [Test]
        public void Process_WhenASubscriberFailsStillNotifiesTheRestOfTheSubscribers()
        {
            bool sub2called = false;
			ReadingPublisher.Subscribe(GetSubscriber((dataPoints) =>
            {
                throw new Exception("Subscriber failed");
            }));
			ReadingPublisher.Subscribe(GetSubscriber((dataPoints) => sub2called = true));

            ReadingPublisher.Process();

            Assert.That(sub2called, Is.True);
        }

        [Test]
        public void Process_WithNoReadings_PassesAnEmptyListOfDataPointsToAllSubscribers()
        {
            IList<FeatureStatistics> passedDataPoints = null;
			ReadingPublisher.Subscribe(GetSubscriber((dataPoints) =>
            {
                passedDataPoints = dataPoints;
            }));

            ReadingPublisher.Process();

            Assert.That(passedDataPoints, Is.Empty);
        }

        [Test]
		[Ignore("Failing test. Rewrite timer implementation to avoid using a real timer during tests.")]
        public void Initialise_100MsForProcessTime_StartsATimeThatCallsAfter100ms()
        {
            bool processed = false;
			ReadingPublisher.Subscribe(GetSubscriber((dataPoints) =>
            {
                processed = true;
            }));

            ReadingPublisher.Start(100);

            Thread.Sleep(300);

            Assert.That(processed, Is.True);
        }

        [Test]
        public void Shutdown_StopsEnsuresProcessIsNotCalledAgain()
        {
            bool processed = false;
            Console.Write("Monitor Subscribers: " + ReadingPublisher.Subscribers.Count);
            Console.Write("processed: " + processed);
			ReadingPublisher.Subscribe(GetSubscriber((dataPoints) =>
            {
                processed = true;
            }));

            ReadingPublisher.Start(100);
            ReadingPublisher.Stop();
            Thread.Sleep(150);

            Assert.That(processed, Is.False);
        }

        [Test]
        public void Process_WithTwoReadingsWithSameNameButDifferentReadingDataNames_SendsOneDataPointToSubscribersWithTwoReadingDataValues()
        {
            IList<FeatureStatistics> passedDataPoints = null;
			ReadingPublisher.Subscribe(GetSubscriber((dataPoints) =>
            {
                passedDataPoints = dataPoints;
            }));
            ReadingPublisher.PublishReading(new Reading { FeatureName = "DataPointName", Data = new Int32ReadingData(1) { Name = "Number" } });
            ReadingPublisher.PublishReading(new Reading { FeatureName = "DataPointName", Data = new Int32ReadingData(1) { Name = "Error" } });

            ReadingPublisher.Start(100, false);
            ReadingPublisher.Process();

            FeatureStatistics dataPoint = passedDataPoints.First();

            Assert.That(dataPoint.Readings.Count, Is.EqualTo(2));
            Assert.That(dataPoint.Readings.First().Name, Is.EqualTo("Number"));
            Assert.That(dataPoint.Readings.Last().Name, Is.EqualTo("Error"));
        }

        [Test]
        public void Process_WithAReading_SendsOneDataPointWithCurrentDate()
        {
            IList<FeatureStatistics> passedDataPoints = null;
			ReadingPublisher.Subscribe(GetSubscriber((dataPoints) =>
            {
                passedDataPoints = dataPoints;
            }));
            ReadingPublisher.PublishReading(new Reading { FeatureName = "DataPointName", Data = new Int32ReadingData(1) { Name = "Number" } });

            ReadingPublisher.Start(100, false);
            ReadingPublisher.Process();

            Assert.That(passedDataPoints.First().Timestamp - DateTime.Now, Is.LessThan(TimeSpan.FromSeconds(1)) );
        }

        private ReadingSubscriber GetSubscriber(Action<IList<FeatureStatistics>> action)
        {
            return new ReadingSubscriber(String.Empty, action);
        }
        private static Reading GetReading()
        {
            return new Reading() { FeatureName = "test", Data = new Int32ReadingData(1) };
        }
    }
}
