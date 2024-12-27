using IndustrialAutomationSuite;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows;

namespace IndustrialAutomationSuite
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private DatabaseHelper dbHelper;
		private BeckhoffServoController servoController;
		private VfdController vfdController;
		private SensorController sensorController;
		private OutputController outputController;
		private SerialDevice serialDevice1;
		private SerialDevice serialDevice2;
		private KeyenceLaserMarkerController laserMarkerController;
		private TurckIOController ioController;
		private DataAcquisitionService dataAcquisitionService;

		public MainWindow()
		{
			InitializeComponent();
			InitializeComponents();
			LoadMachineData();
			StartDataAcquisition();
		}

		private void InitializeComponents()
		{
			// Database connection string
			string connectionString = "your_connection_string_here";
			dbHelper = new DatabaseHelper(connectionString);

			// Select communication strategy
			IServoCommunication servoCommunication = new SerialCommunication("COM3");
			// IServoCommunication servoCommunication = new ProfinetCommunication();
			// IServoCommunication servoCommunication = new EthercatCommunication();

			// Initialize components
			servoController = new BeckhoffServoController(servoCommunication);
			vfdController = new VfdController("COM4");
			sensorController = new SensorController();
			outputController = new OutputController();
			serialDevice1 = new SerialDevice("COM1");
			serialDevice2 = new SerialDevice("COM2");

			// Initialize laser marker
			var laserMarkerCommunication = new SerialCommunication("COM5");
			laserMarkerController = new KeyenceLaserMarkerController(laserMarkerCommunication);

			// Initialize IO controller
			ioController = new TurckIOController("192.168.1.100", 502);

			// Initialize data acquisition service
			dataAcquisitionService = new DataAcquisitionService(sensorController, ioController, vfdController, servoController, dbHelper);
		}

		private void LoadMachineData()
		{
			// Example usage
			servoController.MoveServo(1, 90);
			vfdController.SetSpeed(1000);
			bool sensorState = sensorController.GetSensorState(1);
			outputController.SetOutput(1, true);
			serialDevice1.SendData("Hello Device 1");
			serialDevice2.SendData("Hello Device 2");

			// Laser marker usage
			laserMarkerController.SetMarkingParameters("PARAM1=VALUE1;PARAM2=VALUE2");
			laserMarkerController.StartMarking();
			string markingStatus = laserMarkerController.GetMarkingStatus();
			laserMarkerController.StopMarking();
			laserMarkerController.ResetMarkerAlarm();

			// IO operations
			bool digitalInputState = ioController.ReadDigitalInput(1);
			ioController.WriteDigitalOutput(1, true);
			double analogInputValue = ioController.ReadAnalogInput(1);
			ioController.WriteAnalogOutput(1, 5.0);

			// Update UI with machine data
			ServoPositionText.Text = $"Servo Position: 90";
			VfdSpeedText.Text = $"VFD Speed: 1000";
			SensorStateText.Text = $"Sensor State: {sensorState}";
			DigitalOutputText.Text = $"Digital Output: True";
			AnalogInputText.Text = $"Analog Input: 5.0";

			// Database operations
			List<ParameterRecipe> recipes = dbHelper.GetParameterRecipes();
			foreach (var recipe in recipes)
			{
				Logger.Info($"Recipe: {recipe.RecipeName}, Servo Position: {recipe.ServoPosition}, VFD Speed: {recipe.VfdSpeed}");
			}

			var result = new Results
			{
				RecipeId = 1,
				Timestamp = DateTime.Now,
				Success = true,
				Details = "Operation completed successfully."
			};
			dbHelper.InsertResult(result);

			var alarm = new AlarmLog
			{
				Timestamp = DateTime.Now,
				AlarmMessage = "High temperature detected.",
				Severity = 2
			};
			dbHelper.LogAlarm(alarm);

			var error = new ErrorLog
			{
				Timestamp = DateTime.Now,
				ErrorMessage = "Null reference exception.",
				StackTrace = "Stack trace details here."
			};
			dbHelper.LogError(error);

			// Update UI with diagnostics data
			LastErrorText.Text = $"Last Error: Null reference exception.";
			ErrorTimestampText.Text = $"Error Timestamp: {DateTime.Now}";
			ErrorDetailsText.Text = $"Error Details: Stack trace details here.";
		}

		private void StartDataAcquisition()
		{
			dataAcquisitionService.Start();
		}

		private void StopDataAcquisition()
		{
			dataAcquisitionService.Stop();
		}
	}
}
