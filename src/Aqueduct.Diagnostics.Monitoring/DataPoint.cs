using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class DataPoint
    {
        public DateTime Date { get; private set; }
        public string Name { get; set; }
        public IList<ReadingData> Data { get; private set; }

        public DataPoint()
        {
            Date = DateTime.Now;
            Data = new List<ReadingData>();
        }


    }
}