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
            NotificationProcessor.Initialise(100, false);
            NotificationProcessor.Subscribe(GetSubscriber((dp)
                =>
                _dataPoints.AddRange(dp)
                ));
        }

        [TearDown]
        public void Teardown()
        {
            NotificationProcessor.Reset();
        }

        private MonitorSubscriber GetSubscriber(Action<IList<DataPoint>> action)
        {
            return new MonitorSubscriber("name", action);
        }

        protected Reading GetNumberReading(string name, int value)
        {
            return new Reading() { Name = name, Data = new NumberReadingData(value) };
        }
    }
}

