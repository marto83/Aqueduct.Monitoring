using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class SensorBase
    {
        private const string DataPointNameSlot = "FeatureName";
        protected static string GetDataPointName()
        {
            return (string)Thread.GetData(Thread.GetNamedDataSlot(DataPointNameSlot)) ?? "Application";
        }

        public static void SetDataPointName(string dataPointName)
        {
            Thread.SetData(Thread.GetNamedDataSlot(DataPointNameSlot), dataPointName);
        }

        protected static void AddReading(ReadingData data)
        {
            var newReading = new Reading() { Name = GetDataPointName(), Data = data };
            NotificationProcessor.Add(newReading);
        }
    }
}