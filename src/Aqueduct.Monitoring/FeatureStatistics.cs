using System;
using System.Collections.Generic;
using Aqueduct.Monitoring.Readings;

namespace Aqueduct.Monitoring
{
	public class FeatureStatistics
	{
        public const string GlobalGroupName = "Global";

        public FeatureStatistics()
		{
			Timestamp = DateTimeOffset.Now;
			Readings = new List<ReadingData>();
            Group = string.Empty;
		}

		public DateTimeOffset Timestamp { get; private set; }
		public string Name { get; set; }
        public string Group { get; set; }
		public IList<ReadingData> Readings { get; private set; }
	}
}