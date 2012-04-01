using Aqueduct.Diagnostics.Monitoring.Readings;

namespace Aqueduct.Diagnostics.Monitoring.Sensors
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