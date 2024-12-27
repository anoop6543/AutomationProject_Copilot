using System;

namespace IndustrialAutomationSuite
{
	public struct Results
	{
		public int ResultId { get; set; }
		public int RecipeId { get; set; }
		public DateTime Timestamp { get; set; }
		public bool Success { get; set; }
		public string Details { get; set; }
	}
}
