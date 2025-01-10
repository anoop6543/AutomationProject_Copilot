using NLog;

namespace IndustrialAutomationSuite
{
	public class MockSafetyInterlockController : SafetyInterlockController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public MockSafetyInterlockController() : base(new MockTurckIOController(), new MockEthercatCommunication())
		{
		}

		public override bool AreInterlocksSatisfied()
		{
			Logger.Info("Mock safety interlocks checked.");
			return true; // Mock interlock conditions
		}

		public override bool CheckEmergencyStop()
		{
			Logger.Info("Mock emergency stop checked.");
			return true; // Mock emergency stop condition
		}

		public override bool CheckSafetySwitch()
		{
			Logger.Info("Mock safety switch checked.");
			return true; // Mock safety switch condition
		}

		public override bool CheckEthercatSafetyStatus()
		{
			Logger.Info("Mock EtherCAT safety status checked.");
			return true; // Mock EtherCAT safety status
		}
	}
}
