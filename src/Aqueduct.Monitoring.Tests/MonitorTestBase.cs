using Aqueduct.Monitoring.Sensors;
using NUnit.Framework;

namespace Aqueduct.Monitoring.Tests
{
    public class MonitorTestBase
    {
        [TearDown]
        public void TearDown()
        {
            SensorBase.SetThreadScopedFeatureName(null);
            ReadingPublisher.Reset();
        }
    }
}
