namespace Aqueduct.Monitoring.Readings
{
	public class Int32ReadingData : ReadingData
	{
		public Int32ReadingData(int value)
		{
			Value = value;
		}

		public int Value { get; set; }

		public override dynamic GetValue()
		{
			return Value;
		}

		internal override void Aggregate(ReadingData other)
		{
			Value += (int)other.GetValue();
		}
	}
}