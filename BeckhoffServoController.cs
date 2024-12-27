using NLog;

namespace IndustrialAutomationSuite
{
	public class BeckhoffServoController : BaseServoController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly IServoCommunication _communication;

		public BeckhoffServoController(IServoCommunication communication)
		{
			_communication = communication;
		}

		public override void MoveServo(int servoId, int position)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated moving servo {servoId} to position {position}");
			}
			else
			{
				_communication.SendCommand($"MOVE_SERVO:{servoId}:{position}");
			}
		}

		public override void StartServo(int servoId)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated starting servo {servoId}");
			}
			else
			{
				_communication.SendCommand($"START_SERVO:{servoId}");
			}
		}

		public override void StopServo(int servoId)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated stopping servo {servoId}");
			}
			else
			{
				_communication.SendCommand($"STOP_SERVO:{servoId}");
			}
		}

		public override int GetServoPosition(int servoId)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated getting position of servo {servoId}");
				return 90; // Simulated position
			}
			else
			{
				_communication.SendCommand($"GET_POSITION:{servoId}");
				return int.Parse(_communication.ReadResponse());
			}
		}

		public override string GetServoStatus(int servoId)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated getting status of servo {servoId}");
				return "Simulated status";
			}
			else
			{
				_communication.SendCommand($"GET_STATUS:{servoId}");
				return _communication.ReadResponse();
			}
		}

		public override void ResetServoAlarm(int servoId)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated resetting alarm for servo {servoId}");
			}
			else
			{
				_communication.SendCommand($"RESET_ALARM:{servoId}");
			}
		}
	}
}
