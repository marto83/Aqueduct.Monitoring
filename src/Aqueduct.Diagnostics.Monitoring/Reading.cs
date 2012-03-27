using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class DoubleReadingData : ReadingData
    {
        public double Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the DoubleReadingData class.
        /// </summary>
        public DoubleReadingData(double value)
        {
            Value = value;
        }

        public override object GetValue()
        {
            return Value;
        }

        internal override void Aggregate(ReadingData other)
        {
            Value += (double)other.GetValue();
        }
    }

    public abstract class ReadingData
    {
        public string Name { get; set; }
        public abstract object GetValue();
        internal abstract void Aggregate(ReadingData other);        
    }

    public class Reading
    {
        public string DataPointName { get; set; }
        public ReadingData Data { get; set; }
        public object GetValue() { return Data.GetValue(); }
    }
}

