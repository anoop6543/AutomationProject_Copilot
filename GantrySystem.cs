
using NLog;
using System;
using System.Collections.Generic;

namespace IndustrialAutomationSuite
{
	public class GantrySystem
	{
		private BeckhoffServoController servoController;
		private SensorController sensorController;
		private TurckIOController ioController;
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public GantrySystem(BeckhoffServoController servoController, SensorController sensorController, TurckIOController ioController)
		{
			this.servoController = servoController;
			this.sensorController = sensorController;
			this.ioController = ioController;
		}

		public void HomeAllAxes()
		{
			// Home all axes
			servoController.HomeServo(1);
			servoController.HomeServo(2);
			servoController.HomeServo(3);
			servoController.HomeServo(4);
			servoController.HomeServo(5);
			servoController.HomeServo(6);

			// Wait for all axes to be homed
			WaitForAllAxesToHome();
		}

		public void PickAndPlace_old()
		{
			// Move to pick position
			MoveToPosition(0, 100, 200, 300, 400, 500);
			// Activate gripper
			ioController.WriteDigitalOutput(1, true);
			// Wait for object detection
			while (!sensorController.GetSensorState(1)) { }
			// Move to place position
			MoveToPosition(600, 700, 800, 900, 1000, 1100);
			// Deactivate gripper
			ioController.WriteDigitalOutput(1, false);
		}

		public void PickAndPlace()
		{
			try
			{
				// Home all axes before starting
				HomeAllAxes();

				// Move to pick position
				MoveToPosition(0, 100, 200, 300, 400, 500);

				// Activate gripper
				ioController.WriteDigitalOutput(1, true);

				// Wait for object detection
				WaitForObjectDetection();

				// Move to place position
				MoveToPosition(600, 700, 800, 900, 1000, 1100);

				// Deactivate gripper
				ioController.WriteDigitalOutput(1, false);

				// Return to home position
				HomeAllAxes();
			}
			catch (Exception ex)
			{
				// Handle errors
				HandleError(ex);
			}
		}

		private void MoveToPosition(int x, int y, int z, int a, int b, int c)
		{
			servoController.MoveServo(1, x);
			servoController.MoveServo(2, y);
			servoController.MoveServo(3, z);
			servoController.MoveServo(4, a);
			servoController.MoveServo(5, b);
			servoController.MoveServo(6, c);

			// Wait for all axes to reach the target position
			WaitForAllAxesToReachPosition();
		}

		public void MoveToPosition(int x, int y, int z, int a, int b, int c, int speed = 100)
		{
			servoController.SetSpeed(1, speed);
			servoController.SetSpeed(2, speed);
			servoController.SetSpeed(3, speed);
			servoController.SetSpeed(4, speed);
			servoController.SetSpeed(5, speed);
			servoController.SetSpeed(6, speed);

			servoController.MoveServo(1, x);
			servoController.MoveServo(2, y);
			servoController.MoveServo(3, z);
			servoController.MoveServo(4, a);
			servoController.MoveServo(5, b);
			servoController.MoveServo(6, c);

			// Wait for all axes to reach the target position
			WaitForAllAxesToReachPosition();
		}

		public void MoveThroughWaypoints(List<(int x, int y, int z, int a, int b, int c)> waypoints, int speed = 100)
		{
			foreach (var waypoint in waypoints)
			{
				MoveToPosition(waypoint.x, waypoint.y, waypoint.z, waypoint.a, waypoint.b, waypoint.c, speed);
			}
		}

		private void WaitForAllAxesToHome()
		{
			while (!servoController.IsHomed(1) ||
				   !servoController.IsHomed(2) ||
				   !servoController.IsHomed(3) ||
				   !servoController.IsHomed(4) ||
				   !servoController.IsHomed(5) ||
				   !servoController.IsHomed(6))
			{
				// Wait for a short period before checking again
				System.Threading.Thread.Sleep(100);
			}
		}

		private void WaitForAllAxesToReachPosition()
		{
			while (!servoController.IsAtTargetPosition(1) ||
				   !servoController.IsAtTargetPosition(2) ||
				   !servoController.IsAtTargetPosition(3) ||
				   !servoController.IsAtTargetPosition(4) ||
				   !servoController.IsAtTargetPosition(5) ||
				   !servoController.IsAtTargetPosition(6))
			{
				// Wait for a short period before checking again
				System.Threading.Thread.Sleep(100);
			}
		}

		private void WaitForObjectDetection()
		{
			while (!sensorController.GetSensorState(1))
			{
				// Wait for a short period before checking again
				System.Threading.Thread.Sleep(100);
			}
		}

		private void HandleError(Exception ex)
		{
			// Log the error
			Logger.Error(ex, "An error occurred during the gantry operation.");

			// Perform any necessary cleanup or recovery actions
			// For example, stop all motion and reset the system
			servoController.StopAll();
			ioController.WriteDigitalOutput(1, false);
		}

		public void LogStatus()
		{
			Logger.Info($"Axis 1 Position: {servoController.GetPosition(1)}");
			Logger.Info($"Axis 2 Position: {servoController.GetPosition(2)}");
			Logger.Info($"Axis 3 Position: {servoController.GetPosition(3)}");
			Logger.Info($"Axis 4 Position: {servoController.GetPosition(4)}");
			Logger.Info($"Axis 5 Position: {servoController.GetPosition(5)}");
			Logger.Info($"Axis 6 Position: {servoController.GetPosition(6)}");
		}

	}
}