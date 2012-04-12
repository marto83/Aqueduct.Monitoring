using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Aqueduct.Monitoring.Sensors;
using Aqueduct.Monitoring.Aspects;

namespace Aqueduct.Monitoring.Tests.Aspects
{
    [MonitoredFeature("TestFeature")]
    public class AspectTestClass
    {
        public string FeatureName { get; set; }
        public void Initialise()
        {
            FeatureName = new SensorTestDouble("test").GetFeatureNameExposed().Name;
            //some initialisation code is happending here
            Calucalte();
        }

        public int Calucalte()
        {
            var sum = 10 + 10;
            return sum;
        }

        public void ProcessSomeAction()
        {
            Console.Write("Doing action");
        }

        public void Error()
        {
            throw new Exception();
        }
    }

    [TestFixture]
    public class MonitoredFeatureAspectTests
    {
        [Test]
        public void AutomaticallSetsFeatureNameInThreadContextWhenFirstMethodIsExecuted()
        {
            AspectTestClass aspectTestClass = new AspectTestClass();
            aspectTestClass.Initialise();
            Assert.That(aspectTestClass.FeatureName, Is.EqualTo("TestFeature"));
        }

        [Test]
        public void FeatureNameIsRemovedWhenMethodExits()
        {
            AspectTestClass aspectTestClass = new AspectTestClass();
            aspectTestClass.Initialise();

            Assert.That(new SensorTestDouble("test").GetFeatureNameExposed().Name, Is.EqualTo("Application"));
        }

        [Test]
        public void FeatureNameIsSetOnlyOncePerClass()
        {
            AspectTestClass aspectTestClass = new AspectTestClass();
            aspectTestClass.Initialise();

            Assert.That(new SensorTestDouble("test").GetFeatureNameExposed().Name, Is.EqualTo("Application"));
        }
    }
}
