namespace Aqueduct.Diagnostics.Monitoring.Readings
{
	public class DoubleReadingData : ReadingData
	{
		public DoubleReadingData(double value)
		{
			Value = value;
		}

		public double Value { get; set; }

		public override object GetValue()
		{
			return Value;
		}

		internal override void Aggregate(ReadingData other)
		{
			Value += (double)other.GetValue();
		}
	}
}