namespace MachineAutomation
{
	public struct ErrorLog
	{
		public int ErrorId { get; set; }
		public DateTime Timestamp { get; set; }
		public string ErrorMessage { get; set; }
		public string StackTrace { get; set; }
	}
}
