using NLog;

namespace IndustrialAutomationSuite
{
	public class OutputController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public void SetOutput(int outputId, bool state)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated setting output {outputId} to state {state}");
			}
			else
			{
				// Implement digital output control logic
				Logger.Info($"Setting output {outputId} to state {state}");
			}
		}
	}
}
