using Microsoft.Data.SqlClient;
using NLog;
using System.Collections.Generic;


namespace IndustrialAutomationSuite
{
	public partial class DatabaseHelper
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly string _connectionString;

		public DatabaseHelper(string connectionString)
		{
			_connectionString = connectionString;
		}

		public List<ParameterRecipe> GetParameterRecipes()
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info("Simulated fetching parameter recipes");
				return new List<ParameterRecipe>
					{
						new ParameterRecipe { RecipeId = 1, RecipeName = "Recipe1", ServoPosition = 90.0, VfdSpeed = 1000, MarkingParameters = "PARAM1=VALUE1;PARAM2=VALUE2" },
						new ParameterRecipe { RecipeId = 2, RecipeName = "Recipe2", ServoPosition = 45.0, VfdSpeed = 500, MarkingParameters = "PARAM1=VALUE3;PARAM2=VALUE4" }
					};
			}
			else
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
		}

		public void InsertResult(Results result)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated inserting result: {result.Details}");
			}
			else
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
		}

		public void LogAlarm(AlarmLog alarm)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated logging alarm: {alarm.AlarmMessage}");
			}
			else
			{
				using var connection = new SqlConnection(_connectionString);
				connection.Open();
				using var command = new SqlCommand("INSERT INTO AlarmLog (Timestamp, AlarmMessage, Severity) VALUES (@Timestamp, @AlarmMessage, @Severity)", connection);
				command.Parameters.AddWithValue("@Timestamp", alarm.Timestamp);
				command.Parameters.AddWithValue("@AlarmMessage", alarm.AlarmMessage);
				command.Parameters.AddWithValue("@Severity", alarm.Severity);
				command.ExecuteNonQuery();
			}
		}

		public void LogError(ErrorLog error)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated logging error: {error.ErrorMessage}");
			}
			else
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

		public void LogScadaData(ScadaData data)
		{
			if (Configuration.SimulationMode)
			{
				Logger.Info($"Simulated logging SCADA data: {data}");
			}
			else
			{
				using var connection = new SqlConnection(_connectionString);
				connection.Open();
				using var command = new SqlCommand("INSERT INTO ScadaData (Timestamp, SensorState, AnalogInputValue, VfdSpeed, ServoPosition) VALUES (@Timestamp, @SensorState, @AnalogInputValue, @VfdSpeed, @ServoPosition)", connection);
				command.Parameters.AddWithValue("@Timestamp", data.Timestamp);
				command.Parameters.AddWithValue("@SensorState", data.SensorState);
				command.Parameters.AddWithValue("@AnalogInputValue", data.AnalogInputValue);
				command.Parameters.AddWithValue("@VfdSpeed", data.VfdSpeed);
				command.Parameters.AddWithValue("@ServoPosition", data.ServoPosition);
				command.ExecuteNonQuery();
			}
		}
	}
}
