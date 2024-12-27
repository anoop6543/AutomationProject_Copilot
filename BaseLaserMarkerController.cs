namespace MachineAutomation
{
	public abstract class BaseLaserMarkerController
	{
		public abstract void StartMarking();
		public abstract void StopMarking();
		public abstract void SetMarkingParameters(string parameters);
		public abstract string GetMarkingStatus();
		public abstract void ResetMarkerAlarm();

		protected void Log(string message)
		{
			Console.WriteLine(message);
		}
	}
}
