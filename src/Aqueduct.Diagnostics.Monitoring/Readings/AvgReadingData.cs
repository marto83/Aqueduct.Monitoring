using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.Diagnostics.Monitoring.Readings
{
	public class AvgReadingData : ReadingData
	{
		public AvgReadingData(double value)
		{
			Values = new List<double>();
			Values.Add(value);
		}

		public IList<double> Values { get; private set; }

		public override dynamic GetValue()
		{
			return Values.Average();
		}

		internal override void Aggregate(ReadingData other)
		{
			Values.Add((double)other.GetValue());
		}
	}
}