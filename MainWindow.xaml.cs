using IndustrialAutomationSuite;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.AspNetCore.SignalR.Client;

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
		private EmergencyStopController emergencyStopController;
		private SafetyInterlockController safetyInterlockController;
		private GantrySystem gantrySystem;
		private HubConnection hubConnection;

		public MainWindow()
		{
			InitializeComponent();
			InitializeComponents();
			LoadMachineData();
			StartDataAcquisition();
			gantrySystem = new GantrySystem(servoController, sensorController, ioController);
			InitializeSignalR();
		}

		private void InitializeComponents()
		{
			// Database connection string
			string connectionString = Configuration.DefaultConnectionString;
			dbHelper = new DatabaseHelper(connectionString);

			// Select communication strategy
			IServoCommunication servoCommunication;
			if (Configuration.UseMockHardware)
			{
				servoCommunication = new MockServoCommunication();
			}
			else
			{
				servoCommunication = new SerialCommunication(Configuration.ServoCommunicationPort);
			}

			// Initialize components
			if (Configuration.UseMockHardware)
			{
				servoController = new BeckhoffServoController(servoCommunication);
				vfdController = new MockVfdController();
				sensorController = new MockSensorController();
				outputController = new OutputController();
				serialDevice1 = new SerialDevice(Configuration.SerialDevice1Port);
				serialDevice2 = new SerialDevice(Configuration.SerialDevice2Port);
				laserMarkerController = new MockKeyenceLaserMarkerController();
				ioController = new MockTurckIOController();
				emergencyStopController = new MockEmergencyStopController();
				safetyInterlockController = new MockSafetyInterlockController();
			}
			else
			{
				servoController = new BeckhoffServoController(servoCommunication);
				vfdController = new VfdController(Configuration.VfdCommunicationPort);
				sensorController = new SensorController();
				outputController = new OutputController();
				serialDevice1 = new SerialDevice(Configuration.SerialDevice1Port);
				serialDevice2 = new SerialDevice(Configuration.SerialDevice2Port);
				laserMarkerController = new KeyenceLaserMarkerController(new SerialCommunication(Configuration.LaserMarkerCommunicationPort));
				ioController = new TurckIOController("192.168.1.100", 502);
				emergencyStopController = new EmergencyStopController(ioController, new EthercatCommunication());
				safetyInterlockController = new SafetyInterlockController(ioController, new EthercatCommunication());
			}

			// Add default safety interlocks
			safetyInterlockController.AddDefaultInterlocks();

			// Initialize data acquisition service
			dataAcquisitionService = new DataAcquisitionService(sensorController, ioController, vfdController, servoController, dbHelper);
		}

		private void LoadMachineData()
		{
			// Check safety interlocks
			if (!safetyInterlockController.AreInterlocksSatisfied())
			{
				Logger.Warn("Safety interlocks not satisfied. Aborting operation.");
				return;
			}

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

		private async void StartOperationButton_Click(object sender, RoutedEventArgs e)
		{
			// Start operation logic
			Logger.Info("Starting operation...");
			await gantrySystem.PickAndPlaceAsync();
			// Add your start operation logic here
		}

		private void StopOperationButton_Click(object sender, RoutedEventArgs e)
		{
			// Stop operation logic
			Logger.Info("Stopping operation...");
			// Add your stop operation logic here
		}

		private void ServoPositionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (servoController != null)
			{
				int position = (int)e.NewValue;
				servoController.MoveServo(1, position);
				ServoPositionText.Text = $"Servo Position: {position}";
			}
		}

		private void VfdSpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (vfdController != null)
			{
				int speed = (int)e.NewValue;
				vfdController.SetSpeed(speed);
				VfdSpeedText.Text = $"VFD Speed: {speed}";
			}
		}

		private void AnalogInputSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (ioController != null)
			{
				double value = e.NewValue;
				ioController.WriteAnalogOutput(1, value);
				AnalogInputText.Text = $"Analog Input: {value}";
			}
		}

		private void EmergencyStopButton_Click(object sender, RoutedEventArgs e)
		{
			// Emergency stop logic
			Logger.Warn("Emergency stop activated!");
			emergencyStopController.ActivateEmergencyStop();
		}

		private void ResetEmergencyStopButton_Click(object sender, RoutedEventArgs e)
		{
			// Reset emergency stop logic
			Logger.Info("Resetting emergency stop.");
			emergencyStopController.ResetEmergencyStop();
		}

		private void ModeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (ModeComboBox.SelectedItem != null)
			{
				string selectedMode = (ModeComboBox.SelectedItem as ComboBoxItem).Content.ToString();
				Logger.Info($"Mode changed to: {selectedMode}");
				// Add your mode change logic here
			}
		}

		private void CustomParameterTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			string customParameter = CustomParameterTextBox.Text;
			Logger.Info($"Custom parameter changed to: {customParameter}");
			// Add your custom parameter change logic here
		}

		private async void InitializeSignalR()
		{
			hubConnection = new HubConnectionBuilder()
				.WithUrl(Configuration.SignalRHubUrl)
				.Build();

			hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
			{
				Dispatcher.Invoke(() =>
				{
					// Update UI with received message
					MessagesList.Items.Add($"{user}: {message}");
				});
			});

			try
			{
				await hubConnection.StartAsync();
				Logger.Info("SignalR connection started.");
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Error starting SignalR connection.");
			}
		}

		private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
		{
			if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
			{
				try
				{
					await hubConnection.InvokeAsync("SendMessage", "User", MessageTextBox.Text);
					MessageTextBox.Clear();
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Error sending message via SignalR.");
				}
			}
		}
	}
}
