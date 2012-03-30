using System;
using System.Collections.Generic;
using System.Linq;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class MonitorSubscriber
    {
        public MonitorSubscriber(string name, Action<IList<FeatureStats>> action)
        {
            Name = name;
            Action = action;
        }

        public string Name { get; private set; }
        public Action<IList<FeatureStats>> Action { get; private set; }
    }
}

