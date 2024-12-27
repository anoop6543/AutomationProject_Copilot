using System.IO.Ports;


namespace MachineAutomation
{
	public class VfdController
	{
		private readonly SerialPort _serialPort;

		public VfdController(string portName)
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

		public void SetSpeed(int speed)
		{
			// Implement VFD control logic to set speed
			string command = $"SET_SPEED:{speed}";
			SendCommand(command);
			Console.WriteLine($"Setting VFD speed to {speed}");
		}

		public void Start()
		{
			// Implement VFD control logic to start
			string command = "START";
			SendCommand(command);
			Console.WriteLine("Starting VFD");
		}

		public void Stop()
		{
			// Implement VFD control logic to stop
			string command = "STOP";
			SendCommand(command);
			Console.WriteLine("Stopping VFD");
		}

		public int GetSpeed()
		{
			// Implement VFD control logic to get current speed
			string command = "GET_SPEED";
			string response = SendCommand(command);
			int speed = int.Parse(response);
			Console.WriteLine($"Current VFD speed is {speed}");
			return speed;
		}

		public string GetFaultCode()
		{
			// Implement VFD control logic to get fault code
			string command = "GET_FAULT";
			string faultCode = SendCommand(command);
			Console.WriteLine($"Current VFD fault code is {faultCode}");
			return faultCode;
		}

		public void ResetFault()
		{
			// Implement VFD control logic to reset fault
			string command = "RESET_FAULT";
			SendCommand(command);
			Console.WriteLine("Resetting VFD fault");
		}

		private string SendCommand(string command)
		{
			_serialPort.WriteLine(command);
			string response = _serialPort.ReadLine();
			return response;
		}
	}
}
