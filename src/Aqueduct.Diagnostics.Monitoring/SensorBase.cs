using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Aqueduct.Diagnostics.Monitoring
{
    public abstract class SensorBase
    {
        protected string ReadingName { get; private set; }

        public SensorBase(string readingName)
        {
            ReadingName = readingName;
        }

        private const string DataPointNameSlot = "FeatureName";
        protected static string GetDataPointName()
        {
            return (string)Thread.GetData(Thread.GetNamedDataSlot(DataPointNameSlot)) ?? "Application";
        }

        public static void SetDataPointName(string dataPointName)
        {
            Thread.SetData(Thread.GetNamedDataSlot(DataPointNameSlot), dataPointName);
        }

        protected void AddReading(ReadingData data)
        {
            if(string.IsNullOrEmpty(data.Name))
                data.Name = ReadingName;

            var newReading = new Reading() { DataPointName = GetDataPointName(), Data = data };
            NotificationProcessor.Add(newReading);
        }
    }
}