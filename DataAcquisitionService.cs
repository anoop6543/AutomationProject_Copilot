using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IndustrialAutomationSuite
{
	public class DataAcquisitionService
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly SensorController _sensorController;
		private readonly TurckIOController _ioController;
		private readonly VfdController _vfdController;
		private readonly BeckhoffServoController _servoController;
		private readonly DatabaseHelper _dbHelper;
		private readonly CancellationTokenSource _cancellationTokenSource;

		public DataAcquisitionService(SensorController sensorController, TurckIOController ioController, VfdController vfdController, BeckhoffServoController servoController, DatabaseHelper dbHelper)
		{
			_sensorController = sensorController;
			_ioController = ioController;
			_vfdController = vfdController;
			_servoController = servoController;
			_dbHelper = dbHelper;
			_cancellationTokenSource = new CancellationTokenSource();
		}

		public void Start()
		{
			Task.Run(() => AcquireData(_cancellationTokenSource.Token));
		}

		public void Stop()
		{
			_cancellationTokenSource.Cancel();
		}

		private async Task AcquireData(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					// Acquire data from sensors and devices
					bool sensorState = _sensorController.GetSensorState(1);
					double analogInputValue = _ioController.ReadAnalogInput(1);
					int vfdSpeed = _vfdController.GetSpeed();
					int servoPosition = _servoController.GetServoPosition(1);

					// Log data to the database
					var data = new ScadaData
					{
						Timestamp = DateTime.Now,
						SensorState = sensorState,
						AnalogInputValue = analogInputValue,
						VfdSpeed = vfdSpeed,
						ServoPosition = servoPosition
					};
					_dbHelper.LogScadaData(data);

					// Log data for debugging
					Logger.Info($"Acquired data: {data}");

					// Wait for a specified interval before acquiring data again
					await Task.Delay(1000, cancellationToken); // 1 second interval
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Error during data acquisition");
				}
			}
		}
	}
}
