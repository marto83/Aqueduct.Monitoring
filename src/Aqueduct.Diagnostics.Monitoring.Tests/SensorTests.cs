using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    [TestFixture]
    public class TimingSensorTests
    {
        [Test]
        public void Add_CreatesThreeReadingsWithMinMaxAndAvgReadingData()
        {
            var sensor = new TimingSensor();
            sensor.Add(10.5);

            Assert.That(NotificationProcessor.Readings.Count, Is.EqualTo(3));

            
            Reading reading = null;
            IList<Reading> readings = new List<Reading>();
            for(int i = 0; i < 3; i++){
                NotificationProcessor.Readings.TryDequeue(out reading);
                readings.Add(reading);
            }
            Assert.That(readings.First().Data, Is.InstanceOfType<MinReadingData>());
            Assert.That(readings.Skip(1).First().Data, Is.InstanceOfType<MaxReadingData>());
            Assert.That(readings.Last().Data, Is.InstanceOfType<AvgReadingData>());
        }
    }

    [TestFixture]
    public class CountSensorTests
    {
        [Test]
        public void Increment_AddsNumberReadingToNotificationProcessor()
        {
            var sensor = new CountSensor();
            sensor.Increment();

            Assert.That(NotificationProcessor.Readings.Count, Is.EqualTo(1));

            Reading reading = null;
            NotificationProcessor.Readings.TryDequeue(out reading);
            Assert.That(reading.Data.GetValue(), Is.EqualTo(1));
        }

        [Test]
        public void Increment_WhenNoDataPointNameAvailable_SetsReadingNameTo_Application()
        {
            var sensor = new CountSensor();
            sensor.Increment();

            Reading reading = null;
            NotificationProcessor.Readings.TryDequeue(out reading);
            Assert.That(reading.Name, Is.EqualTo("Application"));
        }

        [Test]
        public void Increment_WhenDataPointNameIsInThreadContext_SetsReadingNameToIt()
        {
            string dataPointName = "datapointName";
            CountSensor.SetDataPointName(dataPointName);

            var sensor = new CountSensor();
            sensor.Increment();

            Reading reading = null;
            NotificationProcessor.Readings.TryDequeue(out reading);
            Assert.That(reading.Name, Is.EqualTo(dataPointName));
        }

        [TearDown]
        public void Teardown()
        {
            CountSensor.SetDataPointName(null);
        }
    }
}
