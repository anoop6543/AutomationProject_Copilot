using NLog;
using System.IO.Ports;


namespace IndustrialAutomationSuite
{
	public class VfdController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly SerialPort _serialPort;

		public VfdController(string portName)
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

		public virtual void SetSpeed(int speed)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated setting VFD speed to {speed}");
			}
			else
			{
				_serialPort.WriteLine($"SET_SPEED:{speed}");
			}
		}

		public virtual void Start()
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info("Simulated starting VFD");
			}
			else
			{
				_serialPort.WriteLine("START");
			}
		}

		public virtual void Stop()
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info("Simulated stopping VFD");
			}
			else
			{
				_serialPort.WriteLine("STOP");
			}
		}

		public virtual int GetSpeed()
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info("Simulated getting VFD speed");
				return 1000; // Simulated speed
			}
			else
			{
				_serialPort.WriteLine("GET_SPEED");
				return int.Parse(_serialPort.ReadLine());
			}
		}

		public virtual string GetFaultCode()
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info("Simulated getting VFD fault code");
				return "Simulated fault code";
			}
			else
			{
				_serialPort.WriteLine("GET_FAULT");
				return _serialPort.ReadLine();
			}
		}

		public virtual void ResetFault()
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info("Simulated resetting VFD fault");
			}
			else
			{
				_serialPort.WriteLine("RESET_FAULT");
			}
		}
	}
}
