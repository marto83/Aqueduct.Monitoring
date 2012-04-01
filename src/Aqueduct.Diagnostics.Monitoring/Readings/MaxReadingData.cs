using System;

namespace Aqueduct.Diagnostics.Monitoring.Readings
{
	public class MaxReadingData : ReadingData
	{
		public MaxReadingData(double value)
		{
			Value = value;
		}

		public double Value { get; private set; }

		public override object GetValue()
		{
			return Value;
		}

		internal override void Aggregate(ReadingData other)
		{
			Value = Math.Max(Value, (double)other.GetValue());
		}
	}
}