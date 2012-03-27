using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Threading;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    [TestFixture]
    public class MonitorTests
    {
        [TearDown]
        public void TearDown()
        {
            NotificationProcessor.Reset();
        }

        [Test]
        public void Add_AddsReadingToMonitor()
        {
            var reading = GetReading();
            NotificationProcessor.Add(reading);

            Assert.AreEqual(1, NotificationProcessor.Readings.Count);
        }

        [Test]
        public void Reset_ClearsAllMonitorReadings()
        {
            var reading = GetReading();
            NotificationProcessor.Add(reading);

            NotificationProcessor.Reset();

            Assert.That(NotificationProcessor.Readings.Count, Is.EqualTo(0));
        }

        [Test]
        public void Process_ClearsReadingsFromReadingsQueue()
        {
            NotificationProcessor.Add(GetReading());
            NotificationProcessor.Add(GetReading());

            NotificationProcessor.Process();

            Assert.That(NotificationProcessor.Readings.Count, Is.EqualTo(0));
        }

        [Test]
        public void Subscribe_RegistersSubscribesToTheMonitor()
        {
            NotificationProcessor.Subscribe(GetSubscriber((dataPoints) => Console.Write("Subscribed")));

            Assert.That(NotificationProcessor.Subscribers.Count, Is.EqualTo(1));
        }

        [Test]
        public void Reset_ClearsAllSubscribers()
        {
            NotificationProcessor.Subscribe(GetSubscriber((dataPoints) => Console.Write("Subscriber")));

            Assert.That(NotificationProcessor.Subscribers, Is.Not.Empty);

            NotificationProcessor.Reset();
            Assert.That(NotificationProcessor.Subscribers, Is.Empty);
        }

        [Test]
        public void Process_CallsAllSubsribers()
        {
            bool sub1called = false;
            bool sub2called = false;
            NotificationProcessor.Subscribe(GetSubscriber((dataPoints) => sub1called = true));
            NotificationProcessor.Subscribe(GetSubscriber((dataPoints) => sub2called = true));

            NotificationProcessor.Process();

            Assert.That(sub1called, Is.True);
            Assert.That(sub2called, Is.True);
        }

        [Test]
        public void Process_WhenASubscriberFailsStillNotifiesTheRestOfTheSubscribers()
        {
            bool sub2called = false;
            NotificationProcessor.Subscribe(GetSubscriber((dataPoints) =>
            {
                throw new Exception("Subscriber failed");
            }));
            NotificationProcessor.Subscribe(GetSubscriber((dataPoints) => sub2called = true));

            NotificationProcessor.Process();

            Assert.That(sub2called, Is.True);
        }

        [Test]
        public void Process_WithNoReadings_PassesAnEmptyListOfDataPointsToAllSubscribers()
        {
            IList<DataPoint> passedDataPoints = null;
            NotificationProcessor.Subscribe(GetSubscriber((dataPoints) =>
            {
                passedDataPoints = dataPoints;
            }));

            NotificationProcessor.Process();

            Assert.That(passedDataPoints, Is.Empty);
        }

        [Test]
        public void Initialise_100MsForProcessTime_StartsATimeThatCallsAfter100ms()
        {
            bool processed = false;
            NotificationProcessor.Subscribe(GetSubscriber((dataPoints) =>
            {
                processed = true;
            }));

            NotificationProcessor.Initialise(100);

            Thread.Sleep(150);

            Assert.That(processed, Is.True);
        }

        [Test]
        public void Shutdown_StopsEnsuresProcessIsNotCalledAgain()
        {
            bool processed = false;
            Console.Write("Monitor Subscribers: " + NotificationProcessor.Subscribers.Count);
            Console.Write("processed: " + processed);
            NotificationProcessor.Subscribe(GetSubscriber((dataPoints) =>
            {
                processed = true;
            }));

            NotificationProcessor.Initialise(100);
            NotificationProcessor.Shutdown();
            Thread.Sleep(150);

            Assert.That(processed, Is.False);
        }

        private MonitorSubscriber GetSubscriber(Action<IList<DataPoint>> action)
        {
            return new MonitorSubscriber(String.Empty, action);
        }
        private static Reading GetReading()
        {
            return new Reading() { Name = "test", Data = new NumberReadingData(1) };
        }
    }
}
