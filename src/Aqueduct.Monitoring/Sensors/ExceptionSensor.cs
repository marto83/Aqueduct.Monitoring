using System;
using Aqueduct.Monitoring.Readings;
using System.Web;

namespace Aqueduct.Monitoring.Sensors
{
	public class ExceptionSensor : SensorBase
	{
		public ExceptionSensor() : base(String.Empty)
		{
		}

		public void AddError(Exception ex)
		{
            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;
                if (httpEx.GetHttpCode() != 404)
                    RecordException(httpEx.GetHttpCode() + httpEx.GetType().Name);
            }
            else
			    RecordException(ex.GetType().Name);
		}

        private void RecordException(string name)
        {
            AddReading(new Int32ReadingData(1) { Name = "TotalExceptions" });
            AddReading(new Int32ReadingData(1) { Name = name });
        }
    }
}