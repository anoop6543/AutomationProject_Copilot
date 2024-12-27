using NLog;
using System.IO.Ports;


namespace IndustrialAutomationSuite
{
	public class SerialDevice
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly SerialPort _serialPort;

		public SerialDevice(string portName)
		{
			if (!Configuration.SimulationMode)
			{
				_serialPort = new SerialPort(portName);
				_serialPort.Open();
			}
		}

		public void SendData(string data)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated sending data to serial port: {data}");
			}
			else
			{
				// Implement serial communication logic
				_serialPort.WriteLine(data);
				Logger.Info($"Sent data to serial port: {data}");
			}
		}
	}
}
