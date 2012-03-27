using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{

    [TestFixture]
    public class CountSensorTests
    {
        private static CountSensor GetSensor()
        {
            return new CountSensor("test");
        }
        [Test]
        public void Increment_AddsNumberReadingToNotificationProcessor()
        {
            var sensor = GetSensor();
            sensor.Increment();

            Assert.That(NotificationProcessor.Readings.Count, Is.EqualTo(1));

            Reading reading = null;
            NotificationProcessor.Readings.TryDequeue(out reading);
            Assert.That(reading.Data.GetValue(), Is.EqualTo(1));
        }

        [Test]
        public void Increment_WhenNoDataPointNameAvailable_SetsReadingNameTo_Application()
        {
            var sensor = GetSensor();
            sensor.Increment();

            Reading reading = null;
            NotificationProcessor.Readings.TryDequeue(out reading);
            Assert.That(reading.DataPointName, Is.EqualTo("Application"));
        }

        [Test]
        public void Increment_WhenDataPointNameIsInThreadContext_SetsReadingNameToIt()
        {
            string dataPointName = "datapointName";
            CountSensor.SetDataPointName(dataPointName);

            var sensor = GetSensor();
            sensor.Increment();

            Reading reading = null;
            NotificationProcessor.Readings.TryDequeue(out reading);
            Assert.That(reading.DataPointName, Is.EqualTo(dataPointName));
        }

        [TearDown]
        public void Teardown()
        {
            CountSensor.SetDataPointName(null);
            NotificationProcessor.Reset();
        }
    }
}
