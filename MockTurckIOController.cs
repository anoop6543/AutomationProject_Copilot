using NLog;

namespace IndustrialAutomationSuite
{
	public class MockTurckIOController : TurckIOController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public MockTurckIOController() : base("MockIPAddress", 502)
		{
		}

		public override bool ReadDigitalInput(int inputId)
		{
			Logger.Info($"Mock reading digital input {inputId}");
			return true; // Mock state
		}

		public override void WriteDigitalOutput(int outputId, bool state)
		{
			Logger.Info($"Mock setting digital output {outputId} to state {state}");
		}

		public override double ReadAnalogInput(int inputId)
		{
			Logger.Info($"Mock reading analog input {inputId}");
			return 5.0; // Mock value
		}

		public override void WriteAnalogOutput(int outputId, double value)
		{
			Logger.Info($"Mock setting analog output {outputId} to value {value}");
		}
	}
}

