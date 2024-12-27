namespace MachineAutomation
{
	public class KeyenceLaserMarkerController : BaseLaserMarkerController
	{
		private readonly IServoCommunication _communication;

		public KeyenceLaserMarkerController(IServoCommunication communication)
		{
			_communication = communication;
		}

		public override void StartMarking()
		{
			string command = "START_MARKING";
			_communication.SendCommand(command);
			Log("Starting laser marking");
		}

		public override void StopMarking()
		{
			string command = "STOP_MARKING";
			_communication.SendCommand(command);
			Log("Stopping laser marking");
		}

		public override void SetMarkingParameters(string parameters)
		{
			string command = $"SET_PARAMETERS:{parameters}";
			_communication.SendCommand(command);
			Log($"Setting marking parameters: {parameters}");
		}

		public override string GetMarkingStatus()
		{
			string command = "GET_STATUS";
			_communication.SendCommand(command);
			string status = _communication.ReadResponse();
			Log($"Current marking status: {status}");
			return status;
		}

		public override void ResetMarkerAlarm()
		{
			string command = "RESET_ALARM";
			_communication.SendCommand(command);
			Log("Resetting laser marker alarm");
		}
	}
}
