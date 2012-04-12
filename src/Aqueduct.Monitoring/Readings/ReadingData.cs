namespace Aqueduct.Monitoring.Readings
{
	public abstract class ReadingData
	{
		public string Name { get; set; }
		public abstract dynamic GetValue();
		internal abstract void Aggregate(ReadingData other);
	}
}