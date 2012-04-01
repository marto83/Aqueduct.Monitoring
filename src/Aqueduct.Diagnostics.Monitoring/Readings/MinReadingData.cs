using System;

namespace Aqueduct.Diagnostics.Monitoring.Readings
{
	public class MinReadingData : ReadingData
	{
		public MinReadingData(double value)
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
			Value = Math.Min(Value, (double)other.GetValue());
		}
	}
}