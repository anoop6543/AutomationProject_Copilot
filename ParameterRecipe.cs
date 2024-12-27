namespace MachineAutomation
{
	public struct ParameterRecipe
	{
		public int RecipeId { get; set; }
		public string RecipeName { get; set; }
		public double ServoPosition { get; set; }
		public int VfdSpeed { get; set; }
		public string MarkingParameters { get; set; }
	}
}
