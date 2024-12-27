using System.IO.Ports;


namespace MachineAutomation
{
	public class SerialDevice
	{
		private readonly SerialPort _serialPort;

		public SerialDevice(string portName)
		{
			_serialPort = new SerialPort(portName);
			_serialPort.Open();
		}

		public void SendData(string data)
		{
			// Implement serial communication logic
			_serialPort.WriteLine(data);
			Console.WriteLine($"Sent data to serial port: {data}");
		}
	}
}
