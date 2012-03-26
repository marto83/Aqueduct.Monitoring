using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;


namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    [TestFixture]
    public class MonitorNumberReadingTests
    {

        private List<DataPoint> _dataPoints;
        [SetUp]
        public void Setup()
        {
            _dataPoints = new List<DataPoint>();
            Monitor.Initialise(100, false);
            Monitor.Subscribe(GetSubscriber((dp)
                =>
                _dataPoints.AddRange(dp)
                ));
        }

        [TearDown]
        public void Teardown()
        {
            Monitor.Reset();
        }

        [Test]
        public void Process_WithMultipleReadings_CombinesTheReadingsIntoDataPoint()
        {
            string readingName = "test";
            Monitor.Add(GetReading(readingName, 1));
            Monitor.Add(GetReading(readingName, 1));

            Monitor.Process();

            Assert.That(_dataPoints.Count, Is.EqualTo(1));
            Assert.That(_dataPoints.First().Name, Is.EqualTo(readingName));
        }

        [Test]
        public void Process_WithMultipleReadings_CombinesTheReadingsData()
        {
            string readingName = "test";
            Monitor.Add(GetReading(readingName, 1));
            Monitor.Add(GetReading(readingName, 1));

            Monitor.Process();

            Assert.That(_dataPoints.Count, Is.EqualTo(1));
            Assert.That(_dataPoints.First().Data, Is.EqualTo(2));
        }

        private Reading GetReading(string name, int value)
        {
            return new Reading { Name = name, Value = value };
        }

        private MonitorSubscriber GetSubscriber(Action<IList<DataPoint>> action)
        {
            return new MonitorSubscriber("name", action);
        }
    }




}

