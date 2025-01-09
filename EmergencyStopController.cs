using NLog;

namespace IndustrialAutomationSuite
{
	public class EmergencyStopController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private bool _isEmergencyStopActivated;
		private readonly TurckIOController _ioController;
		private readonly EthercatCommunication _ethercatCommunication;

		public bool IsEmergencyStopActivated => _isEmergencyStopActivated;

		public EmergencyStopController(TurckIOController ioController, EthercatCommunication ethercatCommunication)
		{
			_ioController = ioController;
			_ethercatCommunication = ethercatCommunication;
		}

		public virtual void ActivateEmergencyStop()
		{
			_isEmergencyStopActivated = true;
			Logger.Warn("Emergency stop activated!");
			StopAllOperations();
		}

		public virtual void ResetEmergencyStop()
		{
			_isEmergencyStopActivated = false;
			Logger.Info("Emergency stop reset.");
			ResumeOperations();
		}

		public virtual void StopAllOperations()
		{
			// Stop all operations immediately
			_ioController.WriteDigitalOutput(1, false); // Example: Stop a motor
			_ethercatCommunication.SendCommand("STOP_ALL");
			Logger.Info("All operations stopped.");
		}

		public virtual void ResumeOperations()
		{
			// Resume operations if safe
			if (CheckSafetyConditions())
			{
				_ioController.WriteDigitalOutput(1, true); // Example: Start a motor
				_ethercatCommunication.SendCommand("RESUME_ALL");
				Logger.Info("All operations resumed.");
			}
			else
			{
				Logger.Warn("Safety conditions not met. Cannot resume operations.");
			}
		}

		public virtual bool CheckSafetyConditions()
		{
			// Check safety conditions before resuming operations
			bool safetyCondition1 = _ioController.ReadDigitalInput(1); // Example: Check if a safety switch is on
			bool safetyCondition2 = _ethercatCommunication.ReadStatus("SAFETY_STATUS") == "OK"; // Example: Check EtherCAT safety status

			return safetyCondition1 && safetyCondition2;
		}
	}
}

