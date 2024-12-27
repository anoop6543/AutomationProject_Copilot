using System.IO.Ports;


namespace MachineAutomation
{
	public class SerialCommunication : IServoCommunication
	{
		private readonly SerialPort _serialPort;

		public SerialCommunication(string portName)
		{
			_serialPort = new SerialPort(portName)
			{
				BaudRate = 9600,
				Parity = Parity.None,
				StopBits = StopBits.One,
				DataBits = 8,
				Handshake = Handshake.None
			};
			_serialPort.Open();
		}

		public void SendCommand(string command)
		{
			_serialPort.WriteLine(command);
		}

		public string ReadResponse()
		{
			return _serialPort.ReadLine();
		}
	}
}
