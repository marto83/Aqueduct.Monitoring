using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class DataPoint
    {
        public string Name { get; set; }
        public ReadingData Data { get; set; }

        public DataPoint()
        {
            
        }

    }
}