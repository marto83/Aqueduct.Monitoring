using System;
using System.Collections.Generic;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class ReadingSubscriber
    {
        public ReadingSubscriber(string name, Action<IList<FeatureStatistics>> statisticsProcessorAction)
        {
            Name = name;
            ProcessStatistics = statisticsProcessorAction;
        }

        public string Name { get; private set; }
        public Action<IList<FeatureStatistics>> ProcessStatistics { get; private set; }
    }
}
