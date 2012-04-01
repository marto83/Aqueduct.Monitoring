using System;
using System.Collections.Generic;
using Aqueduct.Diagnostics.Monitoring.Readings;

namespace Aqueduct.Diagnostics.Monitoring
{
	public class FeatureStatistics
	{
		public FeatureStatistics()
		{
			Timestamp = DateTimeOffset.Now;
			Readings = new List<ReadingData>();
		}

		public DateTimeOffset Timestamp { get; private set; }
		public string Name { get; set; }
		public IList<ReadingData> Readings { get; private set; }
	}
}