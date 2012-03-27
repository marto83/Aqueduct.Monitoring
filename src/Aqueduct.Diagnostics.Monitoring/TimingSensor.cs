using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class TimingSensor : SensorBase
    {
        public TimingSensor(string readingName)
            : base(readingName)
        {
            
        }
        public void Add(double value)
        {
            AddReading(new MinReadingData(value));
            AddReading(new MaxReadingData(value));
            AddReading(new AvgReadingData(value));
        }
    }
}

