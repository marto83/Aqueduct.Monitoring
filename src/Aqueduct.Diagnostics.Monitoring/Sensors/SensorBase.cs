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
            : this(readingName, featureName, string.Empty)
		{
			
		}

        protected SensorBase(string readingName, string featureName, string featureGroup = "")
        {
            ReadingName = readingName;
            FeatureName = featureName;
            FeatureGroup = featureGroup;
        }

		protected string ReadingName { get; private set; }
		protected string FeatureName { get; private set; }
        protected string FeatureGroup { get; private set; }

        protected string GetFeatureName()
		{
			return FeatureName ?? (string)Thread.GetData(Thread.GetNamedDataSlot(FeatureNameSlotName)) ?? "Application";
		}

		/// <summary>
		/// Sets feature name for all sensors in the current thread.
		/// </summary>
		/// <param name="featureName">Feature name to be used for all sensors in the current thread.</param>
		public static void SetThreadScopedFeatureName(string featureName)
		{
			var localDataStoreSlot = Thread.GetNamedDataSlot(FeatureNameSlotName);
			Thread.SetData(localDataStoreSlot, featureName);
		}

        protected void AddReading(ReadingData data)
		{
			if (string.IsNullOrEmpty(data.Name))
				data.Name = ReadingName;

			var newReading = new Reading { FeatureName = GetFeatureName(), FeatureGroup = FeatureGroup, Data = data };
			ReadingPublisher.PublishReading(newReading);
		}
	}
}