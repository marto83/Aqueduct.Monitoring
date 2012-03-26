using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    public class ReadingTestsBase
    {
        protected List<DataPoint> _dataPoints;

        [SetUp]
        public void Setup()
        {
            _dataPoints = new List<DataPoint>();
            Monitor.Initialise(100, false);
            Monitor.Subscribe(GetSubscriber((dp)
                =>
                _dataPoints.AddRange(dp)
                ));
        }

        [TearDown]
        public void Teardown()
        {
            Monitor.Reset();
        }

        private MonitorSubscriber GetSubscriber(Action<IList<DataPoint>> action)
        {
            return new MonitorSubscriber("name", action);
        }

        protected Reading GetNumberReading(string name, int value)
        {
            return new Reading() { Name = name, Data = new NumberReadingData(value) };
        }

        protected Reading GetReading(string name, double value)
        {
            return new Reading() { Name = name, Data = new DoubleReadingData(value) };
        }
    }
}

