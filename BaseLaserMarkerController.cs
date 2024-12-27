using NLog;

namespace TestProjectAnoop
{
	public abstract class BaseLaserMarkerController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public abstract void StartMarking();
		public abstract void StopMarking();
		public abstract void SetMarkingParameters(string parameters);
		public abstract string GetMarkingStatus();
		public abstract void ResetMarkerAlarm();

		protected void Log(string message)
		{
			Logger.Info(message);
		}
	}
}
