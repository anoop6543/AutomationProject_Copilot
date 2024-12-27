using Microsoft.Data.SqlClient;


namespace MachineAutomation
{
	public class DatabaseHelper
	{
		private readonly string _connectionString;

		public DatabaseHelper(string connectionString)
		{
			_connectionString = connectionString;
		}

		public List<ParameterRecipe> GetParameterRecipes()
		{
			var recipes = new List<ParameterRecipe>();
			using var connection = new SqlConnection(_connectionString);
			connection.Open();
			using var command = new SqlCommand("SELECT * FROM ParameterRecipe", connection);
			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				var recipe = new ParameterRecipe
				{
					RecipeId = reader.GetInt32(0),
					RecipeName = reader.GetString(1),
					ServoPosition = reader.GetDouble(2),
					VfdSpeed = reader.GetInt32(3),
					MarkingParameters = reader.GetString(4)
				};
				recipes.Add(recipe);
			}
			return recipes;
		}

		public void InsertResult(Results result)
		{
			using var connection = new SqlConnection(_connectionString);
			connection.Open();
			using var command = new SqlCommand("INSERT INTO Results (RecipeId, Timestamp, Success, Details) VALUES (@RecipeId, @Timestamp, @Success, @Details)", connection);
			command.Parameters.AddWithValue("@RecipeId", result.RecipeId);
			command.Parameters.AddWithValue("@Timestamp", result.Timestamp);
			command.Parameters.AddWithValue("@Success", result.Success);
			command.Parameters.AddWithValue("@Details", result.Details);
			command.ExecuteNonQuery();
		}

		public void LogAlarm(AlarmLog alarm)
		{
			using var connection = new SqlConnection(_connectionString);
			connection.Open();
			using var command = new SqlCommand("INSERT INTO AlarmLog (Timestamp, AlarmMessage, Severity) VALUES (@Timestamp, @AlarmMessage, @Severity)", connection);
			command.Parameters.AddWithValue("@Timestamp", alarm.Timestamp);
			command.Parameters.AddWithValue("@AlarmMessage", alarm.AlarmMessage);
			command.Parameters.AddWithValue("@Severity", alarm.Severity);
			command.ExecuteNonQuery();
		}

		public void LogError(ErrorLog error)
		{
			using var connection = new SqlConnection(_connectionString);
			connection.Open();
			using var command = new SqlCommand("INSERT INTO ErrorLog (Timestamp, ErrorMessage, StackTrace) VALUES (@Timestamp, @ErrorMessage, @StackTrace)", connection);
			command.Parameters.AddWithValue("@Timestamp", error.Timestamp);
			command.Parameters.AddWithValue("@ErrorMessage", error.ErrorMessage);
			command.Parameters.AddWithValue("@StackTrace", error.StackTrace);
			command.ExecuteNonQuery();
		}
	}
}
