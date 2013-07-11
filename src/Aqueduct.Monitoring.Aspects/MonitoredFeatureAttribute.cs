using System;
using System.Linq;
using Aqueduct.Monitoring.Sensors;
using PostSharp.Aspects;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Aqueduct.Monitoring.Aspects
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class MonitoredFeatureAttribute : OnMethodBoundaryAspect //not happy with it yet
    {
        private readonly string _FeatureName;
        private readonly int _random;

        public MonitoredFeatureAttribute(SerializationInfo info, StreamingContext context)
        {

        }

        public MonitoredFeatureAttribute(string featureName) 
        {
            _random = new Random().Next();
            _FeatureName = featureName;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            Debug.WriteLine("Entering method " + args.Method.Name + "  " + _random);
            SensorBase.SetThreadScopedFeatureName(_FeatureName);
            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Debug.WriteLine("Exiting method " + args.Method.Name + "  " + _random);
            SensorBase.ClearThreadScopedFeatureName();
            base.OnExit(args);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            new ExceptionSensor().AddError(args.Exception);
            base.OnException(args);
        }
    }
}
