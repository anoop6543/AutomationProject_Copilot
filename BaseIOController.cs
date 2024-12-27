using NLog;

namespace IndustrialAutomationSuite
{
	public abstract class BaseIOController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public abstract bool ReadDigitalInput(int inputId);
		public abstract void WriteDigitalOutput(int outputId, bool state);
		public abstract double ReadAnalogInput(int inputId);
		public abstract void WriteAnalogOutput(int outputId, double value);

		protected void Log(string message)
		{
			Logger.Info(message);
		}
	}
}
