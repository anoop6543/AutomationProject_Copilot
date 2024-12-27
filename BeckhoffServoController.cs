namespace MachineAutomation
{
	public class BeckhoffServoController : BaseServoController
	{
		private readonly IServoCommunication _communication;

		public BeckhoffServoController(IServoCommunication communication)
		{
			_communication = communication;
		}

		public override void MoveServo(int servoId, int position)
		{
			string command = $"MOVE_SERVO:{servoId}:{position}";
			_communication.SendCommand(command);
			Log($"Moving servo {servoId} to position {position}");
		}

		public override void StartServo(int servoId)
		{
			string command = $"START_SERVO:{servoId}";
			_communication.SendCommand(command);
			Log($"Starting servo {servoId}");
		}

		public override void StopServo(int servoId)
		{
			string command = $"STOP_SERVO:{servoId}";
			_communication.SendCommand(command);
			Log($"Stopping servo {servoId}");
		}

		public override int GetServoPosition(int servoId)
		{
			string command = $"GET_POSITION:{servoId}";
			_communication.SendCommand(command);
			string response = _communication.ReadResponse();
			int position = int.Parse(response);
			Log($"Current position of servo {servoId} is {position}");
			return position;
		}

		public override string GetServoStatus(int servoId)
		{
			string command = $"GET_STATUS:{servoId}";
			_communication.SendCommand(command);
			string status = _communication.ReadResponse();
			Log($"Current status of servo {servoId} is {status}");
			return status;
		}

		public override void ResetServoAlarm(int servoId)
		{
			string command = $"RESET_ALARM:{servoId}";
			_communication.SendCommand(command);
			Log($"Resetting alarm for servo {servoId}");
		}
	}
}
