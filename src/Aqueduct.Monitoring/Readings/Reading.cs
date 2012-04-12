namespace Aqueduct.Monitoring.Readings
{
	public class Reading
	{
		public string FeatureName { get; set; }
        public string FeatureGroup { get; set; }
		public ReadingData Data { get; set; }

		public dynamic GetValue()
		{
			return Data.GetValue();
		}
	}
}