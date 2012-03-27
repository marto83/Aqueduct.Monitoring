using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class NumberReadingData : ReadingData
    {
        public int Value { get; set; }

        public NumberReadingData(int value)
        {
            Value = value;
        }
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

