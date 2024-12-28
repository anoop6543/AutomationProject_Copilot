using NLog;

namespace IndustrialAutomationSuite
{
	public class MockKeyenceLaserMarkerController : KeyenceLaserMarkerController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public MockKeyenceLaserMarkerController() : base(new MockServoCommunication())
		{
		}

		public override void StartMarking()
		{
			Logger.Info("Mock starting laser marking");
		}

		public override void StopMarking()
		{
			Logger.Info("Mock stopping laser marking");
		}

		public override void SetMarkingParameters(string parameters)
		{
			Logger.Info($"Mock setting marking parameters: {parameters}");
		}

		public override string GetMarkingStatus()
		{
			Logger.Info("Mock getting marking status");
			return "Mock status";
		}

		public override void ResetMarkerAlarm()
		{
			Logger.Info("Mock resetting laser marker alarm");
		}
	}
}

