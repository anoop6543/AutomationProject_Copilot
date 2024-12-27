using System;
using System.Collections.Generic;
using System.Device.Gpio;


namespace MachineAutomation
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Machine Automation Project");

			// Database connection string
			string connectionString = "your_connection_string_here";
			var dbHelper = new DatabaseHelper(connectionString);

			// Select communication strategy
			IServoCommunication servoCommunication = new SerialCommunication("COM3");
			// IServoCommunication servoCommunication = new ProfinetCommunication();
			// IServoCommunication servoCommunication = new EthercatCommunication();

			// Initialize components
			var servoController = new BeckhoffServoController(servoCommunication);
			var vfdController = new VfdController("COM4");
			var sensorController = new SensorController();
			var outputController = new OutputController();
			var serialDevice1 = new SerialDevice("COM1");
			var serialDevice2 = new SerialDevice("COM2");

			// Initialize laser marker
			var laserMarkerCommunication = new SerialCommunication("COM5");
			var laserMarkerController = new KeyenceLaserMarkerController(laserMarkerCommunication);

			// Initialize IO controller
			var ioController = new TurckIOController("192.168.1.100", 502);

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

			// Database operations
			List<ParameterRecipe> recipes = dbHelper.GetParameterRecipes();
			foreach (var recipe in recipes)
			{
				Console.WriteLine($"Recipe: {recipe.RecipeName}, Servo Position: {recipe.ServoPosition}, VFD Speed: {recipe.VfdSpeed}");
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
		}
	}
}
