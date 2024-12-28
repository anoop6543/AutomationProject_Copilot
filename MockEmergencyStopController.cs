using NLog;

namespace IndustrialAutomationSuite
{
	public class MockEmergencyStopController : EmergencyStopController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public MockEmergencyStopController() : base(new MockTurckIOController(), new MockEthercatCommunication())
		{
		}

		public override void ActivateEmergencyStop()
		{
			Logger.Warn("Mock emergency stop activated!");
		}

		public override void ResetEmergencyStop()
		{
			Logger.Info("Mock emergency stop reset.");
		}

		protected override void StopAllOperations()
		{
			Logger.Info("Mock all operations stopped.");
		}

		protected override void ResumeOperations()
		{
			Logger.Info("Mock all operations resumed.");
		}

		protected override bool CheckSafetyConditions()
		{
			Logger.Info("Mock safety conditions checked.");
			return true; // Mock safety conditions
		}
	}
}


