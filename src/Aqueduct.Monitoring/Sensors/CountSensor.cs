using Aqueduct.Monitoring.Readings;

namespace Aqueduct.Monitoring.Sensors
{
	public class CountSensor : SensorBase
	{
		public CountSensor(string readingName) : base(readingName)
		{
		}

		public void Increment()
		{
			AddReading(new Int32ReadingData(1));
		}
	}
}