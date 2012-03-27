using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class AmountSensor : SensorBase
    {
        public void Add(double value)
        {
            AddReading(new DoubleReadingData(value));
        }

    }
}

