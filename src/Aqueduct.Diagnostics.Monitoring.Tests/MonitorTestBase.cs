using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    public class MonitorTestBase
    {
        [TearDown]
        public void TearDown()
        {
            NotificationProcessor.Reset();
        }
    }
}
