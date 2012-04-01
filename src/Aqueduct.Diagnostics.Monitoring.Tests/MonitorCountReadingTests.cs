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
            ReadingPublisher.AddReading(GetNumberReading(readingName, 1));
            ReadingPublisher.AddReading(GetNumberReading(readingName, 1));

            ReadingPublisher.Process();

            Assert.That(_dataPoints.Count, Is.EqualTo(1));
            Assert.That(_dataPoints.First().Name, Is.EqualTo(readingName));
        }

        [Test]
        public void Process_WithMultipleReadings_PassesOnOneDataPointWithCombinedDataToSubscribers()
        {
            string readingName = "test";
            ReadingPublisher.AddReading(GetNumberReading(readingName, 1));
            ReadingPublisher.AddReading(GetNumberReading(readingName, 1));

            ReadingPublisher.Process();

            Assert.That(_dataPoints.Count, Is.EqualTo(1));
            Assert.That(_dataPoints.First().Readings.First().GetValue(), Is.EqualTo(2));
        }

        [Test]
        public void Process_With2ReadingsWithDifferentNames_PassesOnTwoSeparateDataPointsToSubscribers()
        {
            ReadingPublisher.AddReading(GetNumberReading("test", 1));
            ReadingPublisher.AddReading(GetNumberReading("test1", 1));

            ReadingPublisher.Process();

            Assert.That(_dataPoints.Count, Is.EqualTo(2));
        }
    }
}

