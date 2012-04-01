using System;
using Aqueduct.Diagnostics.Monitoring.Readings;

namespace Aqueduct.Diagnostics.Monitoring.Sensors
{
	public class ExceptionSensor : SensorBase
	{
		public ExceptionSensor() : base(String.Empty)
		{
		}

		public void AddError(Exception ex)
		{
			AddReading(new Int32ReadingData(1) { Name = "TotalExceptions" });
			AddReading(new Int32ReadingData(1) { Name = ex.GetType().Name });
		}
	}
}