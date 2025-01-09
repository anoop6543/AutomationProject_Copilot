using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndustrialAutomationSuite
{
	public static class Configuration
	{
		public static bool SimulationMode { get; private set; }
		public static bool UseMockHardware { get; private set; }
		public static string DefaultConnectionString { get; private set; }
		public static string ServoCommunicationPort { get; private set; }
		public static string VfdCommunicationPort { get; private set; }
		public static string LaserMarkerCommunicationPort { get; private set; }
		public static string SerialDevice1Port { get; private set; }
		public static string SerialDevice2Port { get; private set; }

		static Configuration()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			var configuration = builder.Build();

			SimulationMode = configuration.GetValue<bool>("SimulationMode");
			UseMockHardware = configuration.GetValue<bool>("UseMockHardware");
			DefaultConnectionString = configuration.GetConnectionString("DefaultConnection");
			ServoCommunicationPort = configuration.GetValue<string>("SerialPorts:ServoCommunication");
			VfdCommunicationPort = configuration.GetValue<string>("SerialPorts:VfdCommunication");
			LaserMarkerCommunicationPort = configuration.GetValue<string>("SerialPorts:LaserMarkerCommunication");
			SerialDevice1Port = configuration.GetValue<string>("SerialPorts:SerialDevice1");
			SerialDevice2Port = configuration.GetValue<string>("SerialPorts:SerialDevice2");
		}
	}
}
