using NLog;

namespace TestProjectAnoop
{
	public class ServoController
	{

		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public void MoveServo(int servoId, int position)
		{
			// Implement servo control logic
			Logger.Info($"Moving servo {servoId} to position {position}");
		}
	}
}
