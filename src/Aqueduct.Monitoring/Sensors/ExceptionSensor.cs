using System;
using Aqueduct.Monitoring.Readings;

namespace Aqueduct.Monitoring.Sensors
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