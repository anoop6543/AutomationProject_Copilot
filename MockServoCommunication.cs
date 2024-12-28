using NLog;

namespace IndustrialAutomationSuite
{
	public class MockServoCommunication : IServoCommunication
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public void SendCommand(string command)
		{
			Logger.Info($"Mock sending command: {command}");
		}

		public string ReadResponse()
		{
			Logger.Info("Mock reading response");
			return "Mock response";
		}
	}
}

