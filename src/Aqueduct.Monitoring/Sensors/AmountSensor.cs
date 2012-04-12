using Aqueduct.Monitoring.Readings;

namespace Aqueduct.Monitoring.Sensors
{
	public class AmountSensor : SensorBase
	{
		public AmountSensor(string readingName) : base(readingName)
		{
		}

		public void Add(double value)
		{
			AddReading(new DoubleReadingData(value));
		}
	}
}