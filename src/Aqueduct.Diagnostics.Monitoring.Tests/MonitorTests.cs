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
            Monitor.Reset();
        }

        [Test]
        public void Add_AddsReadingToMonitor()
        {
            var reading = new Reading();
            Monitor.Add(reading);

            Assert.AreEqual(1, Monitor.Readings.Count);
        }

        [Test]
        public void Reset_ClearsAllMonitorReadings()
        {
            var reading = new Reading();
            Monitor.Add(reading);

            Monitor.Reset();

            Assert.That(Monitor.Readings.Count, Is.EqualTo(0));
        }

        [Test]
        public void Process_ClearsReadingsFromReadingsQueue()
        {
            Monitor.Add(new Reading());
            Monitor.Add(new Reading());

            Monitor.Process();

            Assert.That(Monitor.Readings.Count, Is.EqualTo(0));
        }

        [Test]
        public void Subscribe_RegistersSubscribesToTheMonitor()
        {
            Monitor.Subscribe(GetSubscriber((dataPoints) => Console.Write("Subscribed")));

            Assert.That(Monitor.Subscribers.Count, Is.EqualTo(1));
        }

        [Test]
        public void Reset_ClearsAllSubscribers()
        {
            Monitor.Subscribe(GetSubscriber((dataPoints) => Console.Write("Subscriber")));

            Assert.That(Monitor.Subscribers, Is.Not.Empty);

            Monitor.Reset();
            Assert.That(Monitor.Subscribers, Is.Empty);
        }

        [Test]
        public void Process_CallsAllSubsribers()
        {
            bool sub1called = false;
            bool sub2called = false;
            Monitor.Subscribe(GetSubscriber((dataPoints) => sub1called = true));
            Monitor.Subscribe(GetSubscriber((dataPoints) => sub2called = true));

            Monitor.Process();

            Assert.That(sub1called, Is.True);
            Assert.That(sub2called, Is.True);
        }

        [Test]
        public void Process_WhenASubscriberFailsStillNotifiesTheRestOfTheSubscribers()
        {
            bool sub2called = false;
            Monitor.Subscribe(GetSubscriber((dataPoints) =>
            {
                throw new Exception("Subscriber failed");
            }));
            Monitor.Subscribe(GetSubscriber((dataPoints) => sub2called = true));

            Monitor.Process();

            Assert.That(sub2called, Is.True);
        }

        [Test]
        public void Process_WithNoReadings_PassesAnEmptyListOfDataPointsToAllSubscribers()
        {
            IList<DataPoint> passedDataPoints = null;
            Monitor.Subscribe(GetSubscriber((dataPoints) =>
            {
                passedDataPoints = dataPoints;
            }));

            Monitor.Process();

            Assert.That(passedDataPoints, Is.Empty);
        }

        [Test]
        public void Initialise_100MsForProcessTime_StartsATimeThatCallsAfter100ms()
        {
            bool processed = false;
            Monitor.Subscribe(GetSubscriber((dataPoints) =>
            {
                processed = true;
            }));

            Monitor.Initialise(100);

            Thread.Sleep(150);

            Assert.That(processed, Is.True);
        }

        [Test]
        public void Shutdown_StopsEnsuresProcessIsNotCalledAgain()
        {
            bool processed = false;
            Console.Write("Monitor Subscribers: " + Monitor.Subscribers.Count);
            Console.Write("processed: " + processed);
            Monitor.Subscribe(GetSubscriber((dataPoints) =>
            {
                processed = true;
            }));

            Monitor.Initialise(100);
            Monitor.Shutdown();
            Thread.Sleep(150);

            Assert.That(processed, Is.False);
        }

        private MonitorSubscriber GetSubscriber(Action<IList<DataPoint>> action)
        {
            return new MonitorSubscriber(String.Empty, action);
        }
    }
}
