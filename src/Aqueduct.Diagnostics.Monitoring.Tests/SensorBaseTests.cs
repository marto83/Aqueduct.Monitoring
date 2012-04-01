using System.Linq;
using Aqueduct.Diagnostics.Monitoring.Readings;
using Aqueduct.Diagnostics.Monitoring.Sensors;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    [TestFixture]
    public class SensorBaseTests : MonitorTestBase
    {
        class SensorTestImpl : SensorBase
        {
            public SensorTestImpl(string readingName)
                : base(readingName)
            {

            }

            public SensorTestImpl(string readingName, string featureName)
                : base(readingName, featureName)
            {
                
            }

            public string GetFeatureNameExposed()
            {
                return base.GetFeatureName();
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

        [Test]
        public void AddReading_WhenNoDataPointNameAvailable_SetsReadingNameTo_Application()
        {
            var sensor = new SensorTestImpl("test");

            Assert.That(sensor.GetFeatureNameExposed(), Is.EqualTo("Application"));
        }

        [Test]
        public void GetFeatureName_WhenFeatureNameIsInThreadContext_ReturnsTheOneFromTheThreadContext()
        {
            string featureName = "datapointName";
			CountSensor.SetThreadScopedFeatureName(featureName);

            var sensor = new SensorTestImpl("test");

            Assert.That(sensor.GetFeatureNameExposed(), Is.EqualTo(featureName));
        }

        [Test]
        public void GetFeatureName_WithFeatureName_SetsFeatureNameForAllReadingsAdded()
        {
            string featureName = "Feature1";

            var sensor = new SensorTestImpl("readingName", featureName);

            Assert.That(sensor.GetFeatureNameExposed(), Is.EqualTo(featureName));
        }

    
        
    }
}
