using Aqueduct.Diagnostics.Monitoring.Readings;

namespace Aqueduct.Diagnostics.Monitoring.Sensors
{
    public class TimingSensor : SensorBase
    {
        public TimingSensor(string readingName)
            : base(readingName)
        {
            
        }
        public void Add(double value)
        {
            AddReading(new MinReadingData(value) { Name = ReadingName + " - Min" });
            AddReading(new MaxReadingData(value) { Name = ReadingName + " - Max" });
            AddReading(new AvgReadingData(value) { Name = ReadingName + " - Avg" });
        }
    }
}

