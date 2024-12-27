using NLog;

namespace IndustrialAutomationSuite
{
	public abstract class BaseServoController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public abstract void MoveServo(int servoId, int position);
		public abstract void StartServo(int servoId);
		public abstract void StopServo(int servoId);
		public abstract int GetServoPosition(int servoId);
		public abstract string GetServoStatus(int servoId);
		public abstract void ResetServoAlarm(int servoId);

		protected void Log(string message)
		{
			Logger.Info(message);
		}
	}
}
