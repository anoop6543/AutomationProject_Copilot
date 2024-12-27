using System;


namespace IndustrialAutomationSuite
{

	public struct ScadaData
	{
		public DateTime Timestamp { get; set; }
		public bool SensorState { get; set; }
		public double AnalogInputValue { get; set; }
		public int VfdSpeed { get; set; }
		public int ServoPosition { get; set; }

		public override string ToString()
		{
			return $"Timestamp: {Timestamp}, SensorState: {SensorState}, AnalogInputValue: {AnalogInputValue}, VfdSpeed: {VfdSpeed}, ServoPosition: {ServoPosition}";
		}
	}
}
