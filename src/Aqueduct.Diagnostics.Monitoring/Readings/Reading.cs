namespace Aqueduct.Diagnostics.Monitoring.Readings
{
	public class Reading
	{
		public string FeatureName { get; set; }
		public ReadingData Data { get; set; }

		public object GetValue()
		{
			return Data.GetValue();
		}
	}
}