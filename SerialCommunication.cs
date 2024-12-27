using NLog;
using System.IO.Ports;


namespace IndustrialAutomationSuite
{
	public class SerialCommunication : IServoCommunication
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly SerialPort _serialPort;

		public SerialCommunication(string portName)
		{
			if (!Configuration.SimulationMode)
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
		}

		public void SendCommand(string command)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated sending command: {command}");
			}
			else
			{
				_serialPort.WriteLine(command);
			}
		}

		public string ReadResponse()
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info("Simulated reading response");
				return "Simulated response";
			}
			else
			{
				return _serialPort.ReadLine();
			}
		}
	}
}
