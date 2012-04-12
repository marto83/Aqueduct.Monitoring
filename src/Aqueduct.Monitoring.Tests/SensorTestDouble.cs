using System.Linq;
using Aqueduct.Monitoring.Readings;
using Aqueduct.Monitoring.Sensors;
using System;

namespace Aqueduct.Monitoring.Tests
{
    public class SensorTestDouble : SensorBase
    {
        public SensorTestDouble(string readingName, string featureName = null, string featureGroup = null)
            : base(readingName, featureName, featureGroup)
        {

        }

        internal FeatureDescriptor GetFeatureNameExposed()
        {
            return base.GetFeatureDescriptor();
        }

        public void Add(ReadingData data)
        {
            AddReading(data);
        }
    }
}
