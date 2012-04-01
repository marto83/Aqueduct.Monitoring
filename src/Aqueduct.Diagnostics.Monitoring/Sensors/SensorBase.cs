using System.Threading;
using Aqueduct.Diagnostics.Monitoring.Readings;

namespace Aqueduct.Diagnostics.Monitoring.Sensors
{
	public abstract class SensorBase
	{
		const string FeatureNameSlotName = "FeatureName";

		protected SensorBase(string readingName)
		{
			ReadingName = readingName;
		}

		protected SensorBase(string readingName, string featureName)
		{
			ReadingName = readingName;
			FeatureName = featureName;
		}

		protected string ReadingName { get; private set; }
		protected string FeatureName { get; private set; }

		protected string GetFeatureName()
		{
			return FeatureName ?? (string)Thread.GetData(Thread.GetNamedDataSlot(FeatureNameSlotName)) ?? "Application";
		}

		/// <summary>
		/// Sets feature name for all sensors in the current thread.
		/// </summary>
		/// <param name="featureName">Feature name to be used for all sensors in the current thread.</param>
		public static void SetThreadScopedFeatureName(string featureName)	// NOTE TO MG: Threadwise was not descriptive enough.
		{
			var localDataStoreSlot = Thread.GetNamedDataSlot(FeatureNameSlotName);
			Thread.SetData(localDataStoreSlot, featureName);
		}

		protected void AddReading(ReadingData data)
		{
			if (string.IsNullOrEmpty(data.Name))
				data.Name = ReadingName;

			var newReading = new Reading { FeatureName = GetFeatureName(), Data = data };
			ReadingPublisher.AddReading(newReading);
		}
	}
}