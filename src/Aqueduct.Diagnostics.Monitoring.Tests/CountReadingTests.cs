using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    [TestFixture]
    public class MonitorDoubleReadingTests : ReadingTestsBase
    {
        [Test]
        public void Record_AcceptsReadingWithDoubleValues()
        {
            string readingName = "test";
            Monitor.Add(GetReading(readingName, 10.0));

            Monitor.Process();

            Assert.That(_dataPoints.Count, Is.EqualTo(1));
            Assert.That(_dataPoints.First().Data.GetValue(), Is.EqualTo(10.0));
        }
        
        [Test]
        public void Process_WithTwoDoubleReadings_CalculatesTheCorrectAmountForDataPoint()
        {
            string readingName = "test";
            Monitor.Add(GetReading(readingName, 10.5));
            Monitor.Add(GetReading(readingName, 1.1));

            Monitor.Process();

            Assert.That(_dataPoints.First().Data.GetValue(), Is.EqualTo(10.5 + 1.1));

        }
    }

    [TestFixture]
    public class MonitorCountReadingTests : ReadingTestsBase
    {
        [Test]
        public void Process_WithMultipleReadings_CombinesTheReadingsIntoDataPoint()
        {
            string readingName = "test";
            Monitor.Add(GetNumberReading(readingName, 1));
            Monitor.Add(GetNumberReading(readingName, 1));

            Monitor.Process();

            Assert.That(_dataPoints.Count, Is.EqualTo(1));
            Assert.That(_dataPoints.First().Name, Is.EqualTo(readingName));
        }

        [Test]
        public void Process_WithMultipleReadings_PassesOnOneDataPointWithCombinedDataToSubscribers()
        {
            string readingName = "test";
            Monitor.Add(GetNumberReading(readingName, 1));
            Monitor.Add(GetNumberReading(readingName, 1));

            Monitor.Process();

            Assert.That(_dataPoints.Count, Is.EqualTo(1));
            Assert.That(_dataPoints.First().Data.GetValue(), Is.EqualTo(2));
        }

        [Test]
        public void Process_With2ReadingsWithDifferentNames_PassesOnTwoSeparateDataPointsToSubscribers()
        {
            Monitor.Add(GetNumberReading("test", 1));
            Monitor.Add(GetNumberReading("test1", 1));

            Monitor.Process();

            Assert.That(_dataPoints.Count, Is.EqualTo(2));
        }
    }
}

