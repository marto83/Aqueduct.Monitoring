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
            var sensor = new TimingSensor("test");
            sensor.Add(10.5);

            Assert.That(NotificationProcessor.Readings.Count, Is.EqualTo(3));


            Reading reading = null;
            IList<Reading> readings = new List<Reading>();
            for (int i = 0; i < 3; i++)
            {
                NotificationProcessor.Readings.TryDequeue(out reading);
                readings.Add(reading);
            }
            Assert.That(readings.First().Data, Is.InstanceOfType<MinReadingData>());
            Assert.That(readings.Skip(1).First().Data, Is.InstanceOfType<MaxReadingData>());
            Assert.That(readings.Last().Data, Is.InstanceOfType<AvgReadingData>());
        }

        [TearDown]
        public void TearDown()
        {
            NotificationProcessor.Reset();
        }
    }
}

