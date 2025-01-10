using NLog;
using System;
using System.Collections.Generic;

namespace IndustrialAutomationSuite
{
	public class SafetyInterlockController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly List<Func<bool>> _interlocks;
		private readonly TurckIOController _ioController;
		private readonly EthercatCommunication _ethercatCommunication;

		public SafetyInterlockController(TurckIOController ioController, EthercatCommunication ethercatCommunication)
		{
			_interlocks = new List<Func<bool>>();
			_ioController = ioController;
			_ethercatCommunication = ethercatCommunication;
		}

		public void AddInterlock(Func<bool> interlock)
		{
			_interlocks.Add(interlock);
		}

		public virtual bool AreInterlocksSatisfied()
		{
			foreach (var interlock in _interlocks)
			{
				if (!interlock())
				{
					Logger.Warn("Safety interlock condition not satisfied.");
					return false;
				}
			}
			Logger.Info("All safety interlock conditions satisfied.");
			return true;
		}

		public void AddDefaultInterlocks()
		{
			// Add default interlocks
			AddInterlock(() => CheckEmergencyStop());
			AddInterlock(() => CheckSafetySwitch());
			AddInterlock(() => CheckEthercatSafetyStatus());
		}

		public virtual bool CheckEmergencyStop()
		{
			// Check if the emergency stop is activated
			bool isEmergencyStopActivated = _ioController.ReadDigitalInput(1); // Example: Check emergency stop input
			if (isEmergencyStopActivated)
			{
				Logger.Warn("Emergency stop is activated.");
				return false;
			}
			return true;
		}

		public virtual bool CheckSafetySwitch()
		{
			// Check if a safety switch is on
			bool isSafetySwitchOn = _ioController.ReadDigitalInput(2); // Example: Check safety switch input
			if (!isSafetySwitchOn)
			{
				Logger.Warn("Safety switch is off.");
				return false;
			}
			return true;
		}

		public virtual bool CheckEthercatSafetyStatus()
		{
			// Check EtherCAT safety status
			string safetyStatus = _ethercatCommunication.ReadStatus("SAFETY_STATUS");
			if (safetyStatus != "OK")
			{
				Logger.Warn("EtherCAT safety status is not OK.");
				return false;
			}
			return true;
		}
	}
}

