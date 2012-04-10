using System.Linq;
using Aqueduct.Diagnostics.Monitoring.Readings;
using Aqueduct.Diagnostics.Monitoring.Sensors;
using System;

namespace Aqueduct.Diagnostics.Monitoring.Tests
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
