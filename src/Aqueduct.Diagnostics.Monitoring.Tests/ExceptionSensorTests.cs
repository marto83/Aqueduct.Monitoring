using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    

    [TestFixture]
    public class ExceptionSensorTests : MonitorTestBase
    {
        [Test]
        public void AddException_WithGenericExcetpion_AddTotalExceptionsReadingDataToProcessor()
        {
            var sensor = new ExceptionSensor();

            sensor.AddError(new Exception());

            Reading reading = null;
            NotificationProcessor.Readings.TryDequeue(out reading);

            Assert.That(reading.Data.Name, Is.EqualTo("TotalExceptions"));
        }

        [Test]
        public void Increment_WithException_AddReadingDataForTheSpecificException()
        {
            var sensor = new ExceptionSensor();

            ArgumentException exception = new ArgumentException();
            sensor.AddError(exception);

            Reading reading = null;
            NotificationProcessor.Readings.TryDequeue(out reading); // TotalExceptions
            NotificationProcessor.Readings.TryDequeue(out reading);

            Assert.That(reading.Data.Name, Is.EqualTo(exception.GetType().Name));
        }
    }
}

