namespace Aqueduct.Diagnostics.Monitoring.Readings
{
	public abstract class ReadingData
	{
		public string Name { get; set; }
		public abstract object GetValue();
		internal abstract void Aggregate(ReadingData other);
	}
}