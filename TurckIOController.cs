using System.Net.Sockets;
using System.Text;


namespace MachineAutomation
{
	public class TurckIOController : BaseIOController
	{
		private readonly TcpClient _tcpClient;
		private readonly NetworkStream _networkStream;

		public TurckIOController(string ipAddress, int port)
		{
			_tcpClient = new TcpClient(ipAddress, port);
			_networkStream = _tcpClient.GetStream();
		}

		public override bool ReadDigitalInput(int inputId)
		{
			string command = $"READ_DIGITAL_INPUT:{inputId}";
			string response = SendCommand(command);
			bool state = bool.Parse(response);
			Log($"Digital input {inputId} state is {state}");
			return state;
		}

		public override void WriteDigitalOutput(int outputId, bool state)
		{
			string command = $"WRITE_DIGITAL_OUTPUT:{outputId}:{state}";
			SendCommand(command);
			Log($"Setting digital output {outputId} to state {state}");
		}

		public override double ReadAnalogInput(int inputId)
		{
			string command = $"READ_ANALOG_INPUT:{inputId}";
			string response = SendCommand(command);
			double value = double.Parse(response);
			Log($"Analog input {inputId} value is {value}");
			return value;
		}

		public override void WriteAnalogOutput(int outputId, double value)
		{
			string command = $"WRITE_ANALOG_OUTPUT:{outputId}:{value}";
			SendCommand(command);
			Log($"Setting analog output {outputId} to value {value}");
		}

		private string SendCommand(string command)
		{
			byte[] data = Encoding.ASCII.GetBytes(command);
			_networkStream.Write(data, 0, data.Length);

			byte[] responseData = new byte[256];
			int bytes = _networkStream.Read(responseData, 0, responseData.Length);
			return Encoding.ASCII.GetString(responseData, 0, bytes);
		}
	}
}
