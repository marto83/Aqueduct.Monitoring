using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    public class MonitorTestBase
    {
        [TearDown]
        public void TearDown()
        {
            SensorBase.SetThreadwiseFeatureName(null);
            NotificationProcessor.Reset();
        }
    }
}
