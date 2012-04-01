using Aqueduct.Diagnostics.Monitoring.Readings;

namespace Aqueduct.Diagnostics.Monitoring.Sensors
{
    public class CountSensor : SensorBase
    {
        public CountSensor(string readingName)
            : base(readingName)
        {
            
        }
        public void Increment()
        {
            AddReading(new NumberReadingData(1));
        }
    }
}

