using System;
using System.Collections.Generic;
using Aqueduct.Diagnostics.Monitoring.Readings;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    public class ReadingTestsBase
    {
        protected List<FeatureStats> _dataPoints;

        [SetUp]
        public void Setup()
        {
            _dataPoints = new List<FeatureStats>();
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

        private MonitorSubscriber GetSubscriber(Action<IList<FeatureStats>> action)
        {
            return new MonitorSubscriber("name", action);
        }

        protected Reading GetNumberReading(string name, int value)
        {
            return new Reading() { FeatureName = name, Data = new NumberReadingData(value) };
        }
    }
}

