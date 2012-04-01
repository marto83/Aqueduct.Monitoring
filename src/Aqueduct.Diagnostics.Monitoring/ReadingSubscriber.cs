using System;
using System.Collections.Generic;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class ReadingSubscriber
    {
        public ReadingSubscriber(string name, Action<IList<FeatureStatistics>> action)
        {
            Name = name;
            Action = action;
        }

        public string Name { get; private set; }
        public Action<IList<FeatureStatistics>> Action { get; private set; }
    }
}

