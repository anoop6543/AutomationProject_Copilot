namespace MachineAutomation
{
	public abstract class BaseIOController
	{
		public abstract bool ReadDigitalInput(int inputId);
		public abstract void WriteDigitalOutput(int outputId, bool state);
		public abstract double ReadAnalogInput(int inputId);
		public abstract void WriteAnalogOutput(int outputId, double value);

		protected void Log(string message)
		{
			Console.WriteLine(message);
		}
	}
}
