using Aqueduct.Monitoring.Readings;
using Aqueduct.Monitoring.Sensors;

namespace Aqueduct.Monitoring.Tests
{
    public class SensorTestDouble : SensorBase
    {
        public SensorTestDouble(string readingName) : base(readingName)
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
