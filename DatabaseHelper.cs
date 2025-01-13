using Dapper;
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
				using var connection = new SqlConnection(_connectionString);
				connection.Open();
				var recipes = connection.Query<ParameterRecipe>("SELECT * FROM ParameterRecipe").AsList();
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
				var query = "INSERT INTO Results (RecipeId, Timestamp, Success, Details) VALUES (@RecipeId, @Timestamp, @Success, @Details)";
				connection.Execute(query, result);
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
				var query = "INSERT INTO AlarmLog (Timestamp, AlarmMessage, Severity) VALUES (@Timestamp, @AlarmMessage, @Severity)";
				connection.Execute(query, alarm);
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
				var query = "INSERT INTO ErrorLog (Timestamp, ErrorMessage, StackTrace) VALUES (@Timestamp, @ErrorMessage, @StackTrace)";
				connection.Execute(query, error);
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
				var query = "INSERT INTO ScadaData (Timestamp, SensorState, AnalogInputValue, VfdSpeed, ServoPosition) VALUES (@Timestamp, @SensorState, @AnalogInputValue, @VfdSpeed, @ServoPosition)";
				connection.Execute(query, data);
			}
		}
	}
}
