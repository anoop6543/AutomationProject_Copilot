using NLog;

namespace IndustrialAutomationSuite
{
	public class MockSensorController : SensorController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public override bool GetSensorState(int sensorId)
		{
			Logger.Info($"Mock reading state of sensor {sensorId}");
			return true; // Mock state
		}
	}
}

