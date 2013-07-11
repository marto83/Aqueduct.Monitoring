using System;
using System.Linq;
using Aqueduct.Monitoring.Sensors;
using PostSharp.Aspects;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Aqueduct.Monitoring.Aspects
{

    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MonitorExecutionTimeAttribute : MethodInterceptionAspect
    {
        private readonly string _featureName;
        private readonly string _featureGroup;

        public MonitorExecutionTimeAttribute()
        {

        }

        public MonitorExecutionTimeAttribute(string featureName = null, string featureGroup = null)
        {
            _featureGroup = featureGroup;
            _featureName = featureName;            
        }

        // Record time spent executing the method
        public override void OnInvoke(MethodInterceptionArgs eventArgs)
        {
            string metricName = GetMetricName(
                            eventArgs.Method.DeclaringType,
                            eventArgs.Method.Name,
                            eventArgs.Method.IsGenericMethod,
                            eventArgs.Method.GetGenericArguments());

            var sensor = new TimingSensor(metricName) { FeatureName = _featureName, FeatureGroup = _featureGroup };
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            // continue with method invocation
            eventArgs.Proceed();

            stopwatch.Stop();

            sensor.Add(stopwatch.ElapsedMilliseconds);
        }

        private static string GetMetricName(Type declaringType, string methodName, bool isGenericMethod, Type[] genericArguments)
        {
            if (isGenericMethod)
            {
                return string.Format(
                    "{0}.{1}<{2}>",
                    declaringType.Name,
                    methodName,
                    string.Join(", ", genericArguments.Select(t => t.Name).ToArray()));
            }

            return string.Format("{0}.{1}", declaringType.Name, methodName);
        }
    }
}
