using Aqueduct.Monitoring.Readings;

namespace Aqueduct.Monitoring.Sensors
{
	public class TimingSensor : SensorBase
	{
		public TimingSensor(string readingName) : base(readingName)
		{
		}

		public void Add(double value)
		{
			AddReading(new AvgReadingData(value) { Name = ReadingName + " - Avg ms" });
		}
	}
}