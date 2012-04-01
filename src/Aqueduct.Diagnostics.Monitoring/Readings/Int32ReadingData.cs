namespace Aqueduct.Diagnostics.Monitoring.Readings
{
	public class Int32ReadingData : ReadingData
	{
		public Int32ReadingData(int value)
		{
			Value = value;
		}

		public int Value { get; set; }

		public override object GetValue()
		{
			return Value;
		}

		internal override void Aggregate(ReadingData other)
		{
			Value += (int)other.GetValue();
		}
	}
}