using System;

namespace Aqueduct.Monitoring.Readings
{
	public class MaxReadingData : ReadingData
	{
		public MaxReadingData(double value)
		{
			Value = value;
		}

		public double Value { get; private set; }

		public override dynamic GetValue()
		{
			return Value;
		}

		internal override void Aggregate(ReadingData other)
		{
			Value = Math.Max(Value, (double)other.GetValue());
		}
	}
}