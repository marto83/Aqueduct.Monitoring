using System.Collections.Generic;
using System.Linq;
using Aqueduct.Diagnostics.Monitoring.Readings;
using Aqueduct.Diagnostics.Monitoring.Sensors;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    [TestFixture]
    public class TimingSensorTests : MonitorTestBase
    {
        [Test]
        public void Add_CreatesThreeReadingsWithMinMaxAndAvgReadingData()
        {
            var sensor = new TimingSensor("test");
            sensor.Add(10.5);

            Assert.That(ReadingPublisher.Readings.Count, Is.EqualTo(3));

            Reading reading = null;
            IList<Reading> readings = new List<Reading>();
            for (int i = 0; i < 3; i++)
            {
                ReadingPublisher.Readings.TryDequeue(out reading);
                readings.Add(reading);
            }
            Assert.That(readings.First().Data, Is.InstanceOf<MinReadingData>());
            Assert.That(readings.Skip(1).First().Data, Is.InstanceOf<MaxReadingData>());
            Assert.That(readings.Last().Data, Is.InstanceOf<AvgReadingData>());
        }

        [Test]
        public void Add_CreatesThreeReadingsWithNamesWhichAreACombinationOfSensorNameAndMinMaxAvg()
        {
            string sensorName = "test";
            var sensor = new TimingSensor(sensorName);
            sensor.Add(10.5);

            Assert.That(ReadingPublisher.Readings.Count, Is.EqualTo(3));

            Reading reading = null;
            IList<Reading> readings = new List<Reading>();
            for (int i = 0; i < 3; i++)
            {
                ReadingPublisher.Readings.TryDequeue(out reading);
                readings.Add(reading);
            }
            Assert.That(readings.First().Data.Name, Is.EqualTo(sensorName + " - Min"));
            Assert.That(readings.Skip(1).First().Data.Name, Is.EqualTo(sensorName + " - Max"));
            Assert.That(readings.Last().Data.Name, Is.EqualTo(sensorName + " - Avg"));
        }
    }

    
}