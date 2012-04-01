using System;
using System.Collections.Generic;
using Aqueduct.Diagnostics.Monitoring.Readings;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    public class ReadingTestsBase
    {
        protected List<FeatureStatistics> _dataPoints;

        [SetUp]
        public void Setup()
        {
            _dataPoints = new List<FeatureStatistics>();
            ReadingPublisher.Start(100, false);
			ReadingPublisher.Subscribe(GetSubscriber((dp)
                =>
                _dataPoints.AddRange(dp)
                ));
        }

        [TearDown]
        public void Teardown()
        {
            ReadingPublisher.Reset();
        }

        private ReadingSubscriber GetSubscriber(Action<IList<FeatureStatistics>> action)
        {
            return new ReadingSubscriber("name", action);
        }

        protected Reading GetNumberReading(string name, int value)
        {
            return new Reading() { FeatureName = name, Data = new Int32ReadingData(value) };
        }
    }
}

