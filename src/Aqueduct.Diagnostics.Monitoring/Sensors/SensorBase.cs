using System.Threading;
using Aqueduct.Diagnostics.Monitoring.Readings;
using System;

namespace Aqueduct.Diagnostics.Monitoring.Sensors
{
	public abstract class SensorBase
	{
		const string FeatureNameSlotName = "FeatureName";

		protected SensorBase(string readingName) : this(readingName, null)
		{
			
		}

        protected SensorBase(string readingName, string featureName)
            : this(readingName, featureName, null)
		{
			
		}

        protected SensorBase(string readingName, string featureName, string featureGroup = null)
        {
            ReadingName = readingName;
            FeatureName = featureName;
            FeatureGroup = featureGroup;
        }

		protected string ReadingName { get; private set; }
		protected string FeatureName { get; private set; }
        protected string FeatureGroup { get; private set; }

        internal FeatureDescriptor GetFeatureDescriptor()
        {
            var descriptor = new FeatureDescriptor();
            FeatureDescriptor savedDescriptor = Thread.GetData(Thread.GetNamedDataSlot(FeatureNameSlotName)) as FeatureDescriptor ?? new FeatureDescriptor();
            descriptor.Name = FeatureName ?? savedDescriptor.Name ?? "Application";
            descriptor.Group = FeatureGroup ?? savedDescriptor.Group ?? "Global";
            return descriptor;
        }

		/// <summary>
		/// Sets feature name for all sensors in the current thread.
		/// </summary>
		/// <param name="featureName">Feature name to be used for all sensors in the current thread.</param>
        public static void SetThreadScopedFeatureName(string featureName, string groupName = null)
		{
			var localDataStoreSlot = Thread.GetNamedDataSlot(FeatureNameSlotName);
            Thread.SetData(localDataStoreSlot, new FeatureDescriptor { Name = featureName, Group = groupName });
		}

        public static void ClearThreadScopedFeatureName()
        {
            SetThreadScopedFeatureName(null);
        }

        protected void AddReading(ReadingData data)
        {
            if (string.IsNullOrEmpty(data.Name))
                data.Name = ReadingName;

            FeatureDescriptor descriptor = GetFeatureDescriptor();
            var newReading = new Reading { FeatureName = descriptor.Name, FeatureGroup = descriptor.Group, Data = data };
            ReadingPublisher.PublishReading(newReading);
        }
	}
}