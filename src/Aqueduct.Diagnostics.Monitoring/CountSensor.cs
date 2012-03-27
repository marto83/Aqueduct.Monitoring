using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class CountSensor : SensorBase
    {
        public CountSensor(string readingName)
            : base(readingName)
        {
            
        }
        public void Increment()
        {
            AddReading(new NumberReadingData(1));
        }
    }
}

