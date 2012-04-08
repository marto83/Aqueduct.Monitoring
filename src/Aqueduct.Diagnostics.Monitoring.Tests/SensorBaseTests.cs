using System.Linq;
using Aqueduct.Diagnostics.Monitoring.Readings;
using Aqueduct.Diagnostics.Monitoring.Sensors;
using NUnit.Framework;
using System;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    [TestFixture]
    public class SensorBaseTests : MonitorTestBase
    {
        class SensorTestImpl : SensorBase
        {
            public SensorTestImpl(string readingName, string featureName = null, string featureGroup = "")
                : base(readingName, featureName, featureGroup)
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

            sensor.Add(new Int32ReadingData(1));

            Assert.That(ReadingPublisher.Readings.First().Data.Name, Is.EqualTo("test"));
        }

        [Test]
        public void AddReading_WhenReadingDateNameIsSet_UsesTheReadingDataNameInsteadOfSensorReadingName()
        {
            string readingDataName = "ReadingDataName";
            string sensorReadingName = "SenorReadingName";
            var sensor = new SensorTestImpl(sensorReadingName);

            sensor.Add(new Int32ReadingData(1) { Name = readingDataName });

            Assert.That(ReadingPublisher.Readings.First().Data.Name, Is.EqualTo(readingDataName));
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

        [Test]
        public void GetFeatureGroup_WithoutOneSpecified_SetsGroupToBlank()
        {
            var sensor = new SensorTestImpl("test", "featureName");

            sensor.Add(new Int32ReadingData(1));

            Assert.That(ReadingPublisher.Readings.First().FeatureGroup, Is.EqualTo(string.Empty));
        }

        [Test]
        public void GetFeatureGroup_WithOneSet_SetsCorrectGroupToReading()
        {
            string group = "featureGroup";
            var sensor = new SensorTestImpl("test", "featureName", group);

            sensor.Add(new Int32ReadingData(1));

            Assert.That(ReadingPublisher.Readings.First().FeatureGroup, Is.EqualTo(group));
        }
    
        
    }
}
