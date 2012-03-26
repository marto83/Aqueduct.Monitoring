using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;


namespace Aqueduct.Diagnostics.Monitoring.Tests
{
    [TestFixture]
    public class CountReadingTests
    {
        [TearDown]
        public void Setup()
        {
            Monitor.Reset();
        }

        [Test]
        public void CanAdd()
        {

        }
    }
}

