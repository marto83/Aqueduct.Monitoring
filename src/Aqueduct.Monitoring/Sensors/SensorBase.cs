using System.Threading;
using Aqueduct.Monitoring.Readings;
using System;

namespace Aqueduct.Monitoring.Sensors
{
	public abstract class SensorBase
	{
		const string FeatureNameSlotName = "FeatureName";

		protected SensorBase(string readingName)
		{
            ReadingName = readingName;
		}

		protected string ReadingName { get; private set; }
		public string FeatureName { get; set; }
        public string FeatureGroup { get; set; }

        internal FeatureDescriptor GetFeatureDescriptor()
        {
            var descriptor = new FeatureDescriptor();
            FeatureDescriptor savedDescriptor = Thread.GetData(Thread.GetNamedDataSlot(FeatureNameSlotName)) as FeatureDescriptor ?? new FeatureDescriptor();
            descriptor.Name = FeatureName ?? savedDescriptor.Name ?? "Application";
            descriptor.Group = FeatureGroup ?? savedDescriptor.Group ?? FeatureStatistics.GlobalGroupName;
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