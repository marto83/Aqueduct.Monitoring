using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    [TestFixture]
    public class AmountSensorTests
    {
        [Test]
        public void Add_AddsDoubleReadingToNotificationProcessor()
        {
            var sensor = new AmountSensor();
            double sensorValue = 10.5;
            sensor.Add(sensorValue);

            Assert.That(NotificationProcessor.Readings.Count, Is.EqualTo(1));

            Reading reading = null;
            NotificationProcessor.Readings.TryDequeue(out reading);
            Assert.That(reading.Data.GetValue(), Is.EqualTo(sensorValue));
        }
    }
}

