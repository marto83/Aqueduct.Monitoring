using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    
    [TestFixture]
    public class MaxReadingTests
    {
        [Test]
        public void Aggregate_ReturnTheMaxOfTwoValues()
        {
            var reading1 = new Reading() { Name = "test", Data = new MaxReadingData(1.1) };
            var reading2 = new Reading() { Name = "test", Data = new MaxReadingData(10.5) };

            reading1.Data.Aggregate(reading2.Data);

            Assert.That(reading1.Data.GetValue(), Is.EqualTo(10.5));
        }
    }

    [TestFixture]
    public class MinReadingTests
    {
        [Test]
        public void Aggregate_ReturnsTheMinOfTwoValues()
        {
            var reading1 = new Reading() { Name = "test", Data = new MinReadingData(10.5) };
            var reading2 = new Reading() { Name = "test", Data = new MinReadingData(1.1) };

            reading1.Data.Aggregate(reading2.Data);

            Assert.That(reading1.Data.GetValue(), Is.EqualTo(1.1));
        }
    }

    
    [TestFixture]
    public class AvgReadingTests
    {
        [Test]
        public void Aggregate_ReturnsTheAvegareOfAllAggregatedValues()
        {
            var reading1 = new Reading() { Name = "test", Data = new AvgReadingData(10.0) };
            var reading2 = new Reading() { Name = "test", Data = new AvgReadingData(1.0) };
            var reading3 = new Reading() { Name = "test", Data = new AvgReadingData(4.0) };

            reading1.Data.Aggregate(reading2.Data);
            reading1.Data.Aggregate(reading3.Data);

            Assert.That(reading1.Data.GetValue(), Is.EqualTo(5.0));
        }
    }

    [TestFixture]
    public class MonitorDoubleReadingTests : ReadingTestsBase
    {
        [Test]
        public void Aggregate_AddsUpTheValuesFromTwoDatapoints()
        {
            var reading1 = new Reading() { Name = "test", Data = new DoubleReadingData(10.5) };
            var reading2 = new Reading() { Name = "test", Data = new DoubleReadingData(1.1) };
            reading1.Data.Aggregate(reading2.Data);

            Assert.That(reading1.Data.GetValue(), Is.EqualTo(10.5 + 1.1));

        }
    }
}
