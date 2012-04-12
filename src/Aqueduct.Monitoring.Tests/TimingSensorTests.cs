using System.Collections.Generic;
using System.Linq;
using Aqueduct.Monitoring.Readings;
using Aqueduct.Monitoring.Sensors;
using NUnit.Framework;

namespace Aqueduct.Monitoring.Tests
{
    [TestFixture]
    public class TimingSensorTests : MonitorTestBase
    {
        [Test]
        public void Add_CreatesAvgReading()
        {
            var sensor = new TimingSensor("test");
            sensor.Add(10.5);

            Assert.That(ReadingPublisher.Readings.Count, Is.EqualTo(1));

            Reading reading = null;
            ReadingPublisher.Readings.TryDequeue(out reading);
            Assert.That(reading.Data, Is.InstanceOf<AvgReadingData>());
        }

        [Test]
        public void Add_CreatesAvgReadingWithCorrectName()
        {
            string sensorName = "test";
            var sensor = new TimingSensor(sensorName);
            sensor.Add(10.5);

            Assert.That(ReadingPublisher.Readings.Count, Is.EqualTo(1));

            Reading reading = null;
            ReadingPublisher.Readings.TryDequeue(out reading);
            Assert.That(reading.Data.Name, Is.EqualTo(sensorName + " - Avg ms"));
        }
    }

    
}