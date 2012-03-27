using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    [TestFixture]
    public class AmountSensorTests : MonitorTestBase
    {
        [Test]
        public void Add_AddsDoubleReadingToNotificationProcessor()
        {
            var sensor = new AmountSensor("test");
            double sensorValue = 10.5;
            sensor.Add(sensorValue);

            Assert.That(NotificationProcessor.Readings.Count, Is.EqualTo(1));

            Reading reading = null;
            NotificationProcessor.Readings.TryDequeue(out reading);
            Assert.That(reading.Data.GetValue(), Is.EqualTo(sensorValue));
        }


    }

    [TestFixture]
    public class SensorBaseTests : MonitorTestBase
    {
        class SensorTestImpl : SensorBase
        {
            public SensorTestImpl(string readingName)
                : base(readingName)
            {
                
            }
            
            public void Add(ReadingData data)
            {
                AddReading(data);
            }
        }

        [Test]
        public void AddReading_WhenReadingDataNameNotSet_UsesTheSensorReadingName()
        {
            var sensor = new SensorTestImpl("test");

            sensor.Add(new NumberReadingData(1));

            Assert.That(NotificationProcessor.Readings.First().Data.Name, Is.EqualTo("test"));
        }

        [Test]
        public void AddReading_WhenReadingDateNameIsSet_UsesTheReadingDataNameInsteadOfSensorReadingName()
        {
            string readingDataName = "ReadingDataName";
            string sensorReadingName = "SenorReadingName";
            var sensor = new SensorTestImpl(sensorReadingName);

            sensor.Add(new NumberReadingData(1) { Name = readingDataName });

            Assert.That(NotificationProcessor.Readings.First().Data.Name, Is.EqualTo(readingDataName));
        }
    }
}

