using NLog;

namespace IndustrialAutomationSuite
{
	public class BeckhoffServoController : BaseServoController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly IServoCommunication _communication;
		private readonly int _overloadThreshold = 100; // Example threshold value
		private readonly int _temperatureThreshold = 80; // Example threshold value
		private readonly int _softLimitMin = 0; // Example soft limit minimum position
		private readonly int _softLimitMax = 1000; // Example soft limit maximum position

		public BeckhoffServoController(IServoCommunication communication)
		{
			_communication = communication;
		}

		public override void MoveServo(int servoId, int position)
		{
			Logger.Info($"Attempting to move servo {servoId} to position {position}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated moving servo {servoId} to position {position}");
			}
			else
			{
				if (position < _softLimitMin || position > _softLimitMax)
				{
					Logger.Warn($"Position {position} is outside of soft limits ({_softLimitMin} - {_softLimitMax}). Aborting move.");
					return;
				}

				_communication.SendCommand($"MOVE_SERVO:{servoId}:{position}");
				Logger.Info($"Servo {servoId} moved to position {position}");
			}
		}

		public override void StartServo(int servoId)
		{
			Logger.Info($"Attempting to start servo {servoId}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated starting servo {servoId}");
			}
			else
			{
				_communication.SendCommand($"START_SERVO:{servoId}");
				Logger.Info($"Servo {servoId} started");
			}
		}

		public override void StopServo(int servoId)
		{
			Logger.Info($"Attempting to stop servo {servoId}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated stopping servo {servoId}");
			}
			else
			{
				_communication.SendCommand($"STOP_SERVO:{servoId}");
				Logger.Info($"Servo {servoId} stopped");
			}
		}

		public override int GetServoPosition(int servoId)
		{
			Logger.Info($"Getting position of servo {servoId}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated getting position of servo {servoId}");
				return 90; // Simulated position
			}
			else
			{
				_communication.SendCommand($"GET_POSITION:{servoId}");
				int position = int.Parse(_communication.ReadResponse());
				Logger.Info($"Position of servo {servoId}: {position}");
				return position;
			}
		}

		public override string GetServoStatus(int servoId)
		{
			Logger.Info($"Getting status of servo {servoId}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated getting status of servo {servoId}");
				return "Simulated status";
			}
			else
			{
				_communication.SendCommand($"GET_STATUS:{servoId}");
				string status = _communication.ReadResponse();
				Logger.Info($"Status of servo {servoId}: {status}");
				return status;
			}
		}

		public override void ResetServoAlarm(int servoId)
		{
			Logger.Info($"Attempting to reset alarm for servo {servoId}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated resetting alarm for servo {servoId}");
			}
			else
			{
				_communication.SendCommand($"RESET_ALARM:{servoId}");
				Logger.Info($"Alarm for servo {servoId} reset");
			}
		}

		public void HomeServo(int axis)
		{
			Logger.Info($"Attempting to home axis {axis}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated homing axis {axis}");
			}
			else
			{
				_communication.SendCommand($"HOME_AXIS:{axis}");
				Logger.Info($"Axis {axis} homed");
			}
		}

		public void SetSpeed(int axis, int speed)
		{
			Logger.Info($"Setting speed of axis {axis} to {speed}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated setting speed of axis {axis} to {speed}");
			}
			else
			{
				_communication.SendCommand($"SET_SPEED:{axis}:{speed}");
				Logger.Info($"Speed of axis {axis} set to {speed}");
			}
		}

		public bool IsHomed(int axis)
		{
			Logger.Info($"Checking if axis {axis} is homed");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated checking if axis {axis} is homed");
				return true; // Simulated homed state
			}
			else
			{
				_communication.SendCommand($"IS_HOMED:{axis}");
				bool isHomed = bool.Parse(_communication.ReadResponse());
				Logger.Info($"Axis {axis} homed state: {isHomed}");
				return isHomed;
			}
		}

		public bool IsAtTargetPosition(int axis)
		{
			Logger.Info($"Checking if axis {axis} is at target position");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated checking if axis {axis} is at target position");
				return true; // Simulated target position state
			}
			else
			{
				_communication.SendCommand($"IS_AT_TARGET_POSITION:{axis}");
				bool isAtTarget = bool.Parse(_communication.ReadResponse());
				Logger.Info($"Axis {axis} at target position state: {isAtTarget}");
				return isAtTarget;
			}
		}

		public void StopAll()
		{
			Logger.Info("Attempting to stop all axes");
			if (Configuration.SimulationMode)
			{
				Logger.Info("Simulated stopping all axes");
			}
			else
			{
				_communication.SendCommand("STOP_ALL");
				Logger.Info("All axes stopped");
			}
		}

		public int GetPosition(int axis)
		{
			Logger.Info($"Getting position of axis {axis}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated getting position of axis {axis}");
				return 0; // Simulated position
			}
			else
			{
				_communication.SendCommand($"GET_POSITION:{axis}");
				int position = int.Parse(_communication.ReadResponse());
				Logger.Info($"Position of axis {axis}: {position}");
				return position;
			}
		}

		// Expanded method for auto-referencing
		public void AutoReferenceAxis(int axis, string referenceType)
		{
			Logger.Info($"Starting auto-referencing for axis {axis} using {referenceType}.");

			// Initial safety checks
			if (!IsHomed(axis))
			{
				Logger.Warn($"Axis {axis} is not homed. Homing the axis first.");
				HomeServo(axis);
				if (!IsHomed(axis))
				{
					Logger.Error($"Failed to home axis {axis}. Aborting auto-referencing.");
					return;
				}
			}

			// Set initial speed for referencing
			SetSpeed(axis, 50); // Example speed value

			switch (referenceType)
			{
				case "hard_stop":
					AutoReferenceAgainstHardStop(axis);
					break;
				case "sensor":
					AutoReferenceUsingSensor(axis);
					break;
				default:
					Logger.Warn($"Unknown reference type: {referenceType}");
					break;
			}

			// Final position check
			if (IsAtTargetPosition(axis))
			{
				Logger.Info($"Axis {axis} successfully auto-referenced.");
			}
			else
			{
				Logger.Error($"Axis {axis} failed to reach the target position.");
			}
		}

		private void AutoReferenceAgainstHardStop(int axis)
		{
			Logger.Info($"Auto-referencing axis {axis} against hard stop.");

			// Move axis towards the hard stop
			while (!IsHardStopDetected(axis))
			{
				MoveServo(axis, -1); // Move in negative direction
			}

			// Set the current position as the reference point
			SetReferencePoint(axis);
		}

		private void AutoReferenceUsingSensor(int axis)
		{
			Logger.Info($"Auto-referencing axis {axis} using sensor.");

			// Move axis towards the sensor
			while (!IsSensorTriggered(axis))
			{
				MoveServo(axis, 1); // Move in positive direction
			}

			// Set the current position as the reference point
			SetReferencePoint(axis);
		}

		private bool IsHardStopDetected(int axis)
		{
			Logger.Info($"Checking if hard stop is detected for axis {axis}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated checking if hard stop is detected for axis {axis}");
				return false; // Simulated hard stop detection
			}
			else
			{
				_communication.SendCommand($"IS_HARD_STOP_DETECTED:{axis}");
				bool isHardStopDetected = bool.Parse(_communication.ReadResponse());
				Logger.Info($"Hard stop detected for axis {axis}: {isHardStopDetected}");
				return isHardStopDetected;
			}
		}

		private bool IsSensorTriggered(int axis)
		{
			Logger.Info($"Checking if sensor is triggered for axis {axis}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated checking if sensor is triggered for axis {axis}");
				return false; // Simulated sensor trigger state
			}
			else
			{
				_communication.SendCommand($"IS_SENSOR_TRIGGERED:{axis}");
				bool isSensorTriggered = bool.Parse(_communication.ReadResponse());
				Logger.Info($"Sensor triggered for axis {axis}: {isSensorTriggered}");
				return isSensorTriggered;
			}
		}

		private void SetReferencePoint(int axis)
		{
			Logger.Info($"Setting reference point for axis {axis}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated setting reference point for axis {axis}");
			}
			else
			{
				_communication.SendCommand($"SET_REFERENCE_POINT:{axis}");
				Logger.Info($"Reference point set for axis {axis}");
			}
		}

		// Additional safety features

		public bool CheckOverload(int axis)
		{
			Logger.Info($"Checking overload for axis {axis}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated checking overload for axis {axis}");
				return false; // Simulated overload state
			}
			else
			{
				_communication.SendCommand($"CHECK_OVERLOAD:{axis}");
				bool isOverloaded = bool.Parse(_communication.ReadResponse());
				if (isOverloaded)
				{
					Logger.Warn($"Overload detected for axis {axis}");
					StopServo(axis);
				}
				return isOverloaded;
			}
		}

		public bool CheckTemperature(int axis)
		{
			Logger.Info($"Checking temperature for axis {axis}");
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated checking temperature for axis {axis}");
				return false; // Simulated temperature state
			}
			else
			{
				_communication.SendCommand($"CHECK_TEMPERATURE:{axis}");
				int temperature = int.Parse(_communication.ReadResponse());
				if (temperature > _temperatureThreshold)
				{
					Logger.Warn($"High temperature detected for axis {axis}: {temperature}°C");
					StopServo(axis);
					return true;
				}
				return false;
			}
		}

		public void PerformPeriodicSafetyChecks(int axis)
		{
			Logger.Info($"Performing periodic safety checks for axis {axis}");

			if (CheckOverload(axis))
			{
				Logger.Warn($"Overload condition detected for axis {axis}. Stopping operation.");
				return;
			}

			if (CheckTemperature(axis))
			{
				Logger.Warn($"High temperature condition detected for axis {axis}. Stopping operation.");
				return;
			}

			Logger.Info($"Periodic safety checks completed for axis {axis}");
		}
	}
}
