using Aqueduct.Monitoring.Readings;
using Aqueduct.Monitoring.Sensors;
using NUnit.Framework;

namespace Aqueduct.Monitoring.Tests
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

            Assert.That(ReadingPublisher.Readings.Count, Is.EqualTo(1));

            Reading reading = null;
            ReadingPublisher.Readings.TryDequeue(out reading);
            Assert.That(reading.Data.GetValue(), Is.EqualTo(1));
        }

        

        [TearDown]
        public void Teardown()
        {
            CountSensor.SetThreadScopedFeatureName(null);
            ReadingPublisher.Reset();
        }
    }
}
