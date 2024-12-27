namespace MachineAutomation
{
	public struct AlarmLog
	{
		public int AlarmId { get; set; }
		public DateTime Timestamp { get; set; }
		public string AlarmMessage { get; set; }
		public int Severity { get; set; }
	}
}
