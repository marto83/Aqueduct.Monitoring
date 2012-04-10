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
        

        [Test]
        public void AddReading_WhenReadingDataNameNotSet_UsesTheSensorReadingName()
        {
            var sensor = new SensorTestDouble("test");

            sensor.Add(new Int32ReadingData(1));

            Assert.That(ReadingPublisher.Readings.First().Data.Name, Is.EqualTo("test"));
        }

        [Test]
        public void AddReading_WhenReadingDateNameIsSet_UsesTheReadingDataNameInsteadOfSensorReadingName()
        {
            string readingDataName = "ReadingDataName";
            string sensorReadingName = "SenorReadingName";
            var sensor = new SensorTestDouble(sensorReadingName);

            sensor.Add(new Int32ReadingData(1) { Name = readingDataName });

            Assert.That(ReadingPublisher.Readings.First().Data.Name, Is.EqualTo(readingDataName));
        }

        [Test]
        public void AddReading_WhenNoDataPointNameAvailable_SetsReadingNameTo_Application()
        {
            var sensor = new SensorTestDouble("test");

            Assert.That(sensor.GetFeatureNameExposed().Name, Is.EqualTo("Application"));
        }

        [Test]
        public void GetFeatureName_WhenFeatureNameIsInThreadContext_ReturnsTheOneFromTheThreadContext()
        {
            string featureName = "datapointName";
			SensorBase.SetThreadScopedFeatureName(featureName);

            var sensor = new SensorTestDouble("test");

            Assert.That(sensor.GetFeatureNameExposed().Name, Is.EqualTo(featureName));
        }

        [Test]
        public void ClearThreadScoredFeatureName_AfterSettingFeatureNameToTest_ReturnsApplication()
        {
            string featureName = "datapointName";
            SensorBase.SetThreadScopedFeatureName(featureName);

            var sensor = new SensorTestDouble("test");
            SensorBase.ClearThreadScopedFeatureName();

            Assert.That(sensor.GetFeatureNameExposed().Name, Is.EqualTo("Application"));
        }

        [Test]
        public void GetFeatureName_WithFeatureName_SetsFeatureNameForAllReadingsAdded()
        {
            string featureName = "Feature1";

            var sensor = new SensorTestDouble("readingName", featureName);

            Assert.That(sensor.GetFeatureNameExposed().Name, Is.EqualTo(featureName));
        }

        [Test]
        public void GetFeatureGroup_WithoutOneSpecified_SetsGroupToBlank()
        {
            var sensor = new SensorTestDouble("test", "featureName");

            sensor.Add(new Int32ReadingData(1));

            Assert.That(ReadingPublisher.Readings.First().FeatureGroup, Is.EqualTo(string.Empty));
        }

        [Test]
        public void GetFeatureGroup_WithOneSet_SetsCorrectGroupToReading()
        {
            string group = "featureGroup";
            var sensor = new SensorTestDouble("test", "featureName", group);

            sensor.Add(new Int32ReadingData(1));

            Assert.That(ReadingPublisher.Readings.First().FeatureGroup, Is.EqualTo(group));
        }

        [Test]
        public void SetThreadScopedFeatureName_WithoutFeatureGroupSpecified_SetsBlankFeatureGroup()
        {
            SensorBase.SetThreadScopedFeatureName("test");

            var sensor = new SensorTestDouble("test");

            Assert.That(sensor.GetFeatureNameExposed().Group, Is.EqualTo(string.Empty));
        }

        [Test]
        public void SetThreadScopeFeatureName_WhenFeatureGroupIsSetBothInSensorAndInThreadContext_TheSensorValueIsUsed()
        {
            SensorBase.SetThreadScopedFeatureName("test", "threadScopedGroup");

            string sensorGroup = "SensorGroup";
            var sensor = new SensorTestDouble("test", "Name", sensorGroup);

            Assert.That(sensor.GetFeatureNameExposed().Group, Is.EqualTo(sensorGroup));
        }

        [Test]
        public void SetThreadScopeFeatureName_WithFeatureGroupSpecified_SetsFeatureGroupAccordingly()
        {
            string testGroup = "testGroup";
            SensorBase.SetThreadScopedFeatureName("test", testGroup);

            var sensor = new SensorTestDouble("test");

            Assert.That(sensor.GetFeatureNameExposed().Group, Is.EqualTo(testGroup));
        }
    
        
    }
}
