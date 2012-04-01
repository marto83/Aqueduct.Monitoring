using Aqueduct.Diagnostics.Monitoring.Sensors;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    public class MonitorTestBase
    {
        [TearDown]
        public void TearDown()
        {
            SensorBase.SetThreadScopedFeatureName(null);
            NotificationProcessor.Reset();
        }
    }
}
