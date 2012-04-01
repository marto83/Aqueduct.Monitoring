using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    [TestFixture]
    public class MonitorCountReadingTests : ReadingTestsBase
    {
        [Test]
        public void Process_WithMultipleReadings_CombinesTheReadingsIntoDataPoint()
        {
            string readingName = "test";
            NotificationProcessor.Add(GetNumberReading(readingName, 1));
            NotificationProcessor.Add(GetNumberReading(readingName, 1));

            NotificationProcessor.Process();

            Assert.That(_dataPoints.Count, Is.EqualTo(1));
            Assert.That(_dataPoints.First().Name, Is.EqualTo(readingName));
        }

        [Test]
        public void Process_WithMultipleReadings_PassesOnOneDataPointWithCombinedDataToSubscribers()
        {
            string readingName = "test";
            NotificationProcessor.Add(GetNumberReading(readingName, 1));
            NotificationProcessor.Add(GetNumberReading(readingName, 1));

            NotificationProcessor.Process();

            Assert.That(_dataPoints.Count, Is.EqualTo(1));
            Assert.That(_dataPoints.First().Readings.First().GetValue(), Is.EqualTo(2));
        }

        [Test]
        public void Process_With2ReadingsWithDifferentNames_PassesOnTwoSeparateDataPointsToSubscribers()
        {
            NotificationProcessor.Add(GetNumberReading("test", 1));
            NotificationProcessor.Add(GetNumberReading("test1", 1));

            NotificationProcessor.Process();

            Assert.That(_dataPoints.Count, Is.EqualTo(2));
        }
    }
}

