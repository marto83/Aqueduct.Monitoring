using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Aqueduct.Diagnostics.Monitoring
{
    public abstract class SensorBase
    {
        protected string ReadingName { get; private set; }
        protected string FeatureName { get; private set; }

        public SensorBase(string readingName)
        {
            ReadingName = readingName;
        }

        public SensorBase(string readingName, string featureName)
        {
            ReadingName = readingName;
            FeatureName = featureName;
        }

        private const string FeatureNameSlotName = "FeatureName";
        protected string GetFeatureName()
        {
            return FeatureName ?? (string)Thread.GetData(Thread.GetNamedDataSlot(FeatureNameSlotName)) ?? "Application";
        }

        /// <summary>
        /// Once set all sensors in the current thread can automatically pick up the feature name
        /// </summary>
        /// <param name="featureName">FeatureName used for all readings</param>
        public static void SetThreadwiseFeatureName(string featureName)
        {
            Thread.SetData(Thread.GetNamedDataSlot(FeatureNameSlotName), featureName);
        }

        protected void AddReading(ReadingData data)
        {
            if(string.IsNullOrEmpty(data.Name))
                data.Name = ReadingName;

            var newReading = new Reading() { FeatureName = GetFeatureName(), Data = data };
            NotificationProcessor.Add(newReading);
        }
    }
}