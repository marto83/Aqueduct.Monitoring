using System;

namespace Aqueduct.Diagnostics.Monitoring
{
    public class ExceptionSensor : SensorBase
    {
        public void AddError(Exception ex)
        {
            AddReading(new NumberReadingData(1) { Name = "TotalExceptions" });
            AddReading(new NumberReadingData(1) { Name = ex.GetType().Name });
        }

        public ExceptionSensor()
            : base(String.Empty)
        {

        }
    }
}

