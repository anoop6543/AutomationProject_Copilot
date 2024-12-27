using NLog;
using System.Net.Sockets;
using System.Text;


namespace TestProjectAnoop
{
	public class TurckIOController : BaseIOController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly TcpClient _tcpClient;
		private readonly NetworkStream _networkStream;

		public TurckIOController(string ipAddress, int port)
		{
			if (!Configuration.SimulationMode)
			{
				_tcpClient = new TcpClient(ipAddress, port);
				_networkStream = _tcpClient.GetStream();
			}
		}

		public override bool ReadDigitalInput(int inputId)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated reading digital input {inputId}");
				return true; // Simulated state
			}
			else
			{
				string command = $"READ_DIGITAL_INPUT:{inputId}";
				string response = SendCommand(command);
				bool state = bool.Parse(response);
				Logger.Info($"Digital input {inputId} state is {state}");
				return state;
			}
		}

		public override void WriteDigitalOutput(int outputId, bool state)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated setting digital output {outputId} to state {state}");
			}
			else
			{
				string command = $"WRITE_DIGITAL_OUTPUT:{outputId}:{state}";
				SendCommand(command);
				Logger.Info($"Setting digital output {outputId} to state {state}");
			}
		}

		public override double ReadAnalogInput(int inputId)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated reading analog input {inputId}");
				return 5.0; // Simulated value
			}
			else
			{
				string command = $"READ_ANALOG_INPUT:{inputId}";
				string response = SendCommand(command);
				double value = double.Parse(response);
				Logger.Info($"Analog input {inputId} value is {value}");
				return value;
			}
		}

		public override void WriteAnalogOutput(int outputId, double value)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated setting analog output {outputId} to value {value}");
			}
			else
			{
				string command = $"WRITE_ANALOG_OUTPUT:{outputId}:{value}";
				SendCommand(command);
				Logger.Info($"Setting analog output {outputId} to value {value}");
			}
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
