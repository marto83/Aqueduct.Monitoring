using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class AvgReadingData : ReadingData
    {
        public IList<double> Values { get; private set; }

        public AvgReadingData(double value)
        {
            Values = new List<double>();
            Values.Add(value);
        }

        public override object GetValue()
        {
            return Values.Average();
        }

        internal override void Aggregate(ReadingData other)
        {
            Values.Add((double)other.GetValue());
        }
    }
}

