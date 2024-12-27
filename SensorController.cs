using NLog;

namespace TestProjectAnoop
{
	public class SensorController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public bool GetSensorState(int sensorId)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated reading state of sensor {sensorId}");
				return true; // Simulated state
			}
			else
			{
				// Implement sensor state reading logic
				Logger.Info($"Reading state of sensor {sensorId}");
				return false; // Placeholder return value
			}
		}
	}
}
