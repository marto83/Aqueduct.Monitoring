using System;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class MinReadingData : ReadingData
    {
        public double Value { get; set; }

        public MinReadingData(double value)
        {
            Value = value;
        }

        public override object GetValue()
        {
            return Value;
        }

        internal override void Aggregate(ReadingData other)
        {
            Value = Math.Min(Value, (double)other.GetValue());
        }
    }
}

