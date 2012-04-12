using Aqueduct.Monitoring.Readings;
using Aqueduct.Monitoring.Sensors;
using NUnit.Framework;

namespace Aqueduct.Monitoring.Tests
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

            Assert.That(ReadingPublisher.Readings.Count, Is.EqualTo(1));

            Reading reading = null;
            ReadingPublisher.Readings.TryDequeue(out reading);
            Assert.That(reading.Data.GetValue(), Is.EqualTo(sensorValue));
        }


    }
}
