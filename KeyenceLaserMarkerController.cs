using NLog;

namespace TestProjectAnoop
{
	public class KeyenceLaserMarkerController : BaseLaserMarkerController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly IServoCommunication _communication;

		public KeyenceLaserMarkerController(IServoCommunication communication)
		{
			_communication = communication;
		}

		public override void StartMarking()
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info("Simulated starting laser marking");
			}
			else
			{
				_communication.SendCommand("START_MARKING");
			}
		}

		public override void StopMarking()
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info("Simulated stopping laser marking");
			}
			else
			{
				_communication.SendCommand("STOP_MARKING");
			}
		}

		public override void SetMarkingParameters(string parameters)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated setting marking parameters: {parameters}");
			}
			else
			{
				_communication.SendCommand($"SET_PARAMETERS:{parameters}");
			}
		}

		public override string GetMarkingStatus()
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info("Simulated getting marking status");
				return "Simulated status";
			}
			else
			{
				_communication.SendCommand("GET_STATUS");
				return _communication.ReadResponse();
			}
		}

		public override void ResetMarkerAlarm()
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info("Simulated resetting laser marker alarm");
			}
			else
			{
				_communication.SendCommand("RESET_ALARM");
			}
		}
	}
}
