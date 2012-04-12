namespace Aqueduct.Monitoring.Readings
{
	public class DoubleReadingData : ReadingData
	{
		public DoubleReadingData(double value)
		{
			Value = value;
		}

		public double Value { get; set; }

		public override dynamic GetValue()
		{
			return Value;
		}

		internal override void Aggregate(ReadingData other)
		{
			Value += (double)other.GetValue();
		}
	}
}