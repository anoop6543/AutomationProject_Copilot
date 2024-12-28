using NLog;

namespace IndustrialAutomationSuite
{
	public class MockVfdController : VfdController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public MockVfdController() : base("MockPort")
		{
		}

		public override void SetSpeed(int speed)
		{
			Logger.Info($"Mock setting VFD speed to {speed}");
		}

		public override void Start()
		{
			Logger.Info("Mock starting VFD");
		}

		public override void Stop()
		{
			Logger.Info("Mock stopping VFD");
		}

		public override int GetSpeed()
		{
			Logger.Info("Mock getting VFD speed");
			return 1000; // Mock speed
		}

		public override string GetFaultCode()
		{
			Logger.Info("Mock getting VFD fault code");
			return "Mock fault code";
		}

		public override void ResetFault()
		{
			Logger.Info("Mock resetting VFD fault");
		}
	}
}

