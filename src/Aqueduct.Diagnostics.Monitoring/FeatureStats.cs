using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class FeatureStats
    {
        public DateTime Date { get; private set; }
        public string Name { get; set; }
        public IList<ReadingData> Readings { get; private set; }

        public FeatureStats()
        {
            Date = DateTime.Now;
            Readings = new List<ReadingData>();
        }
    }
}