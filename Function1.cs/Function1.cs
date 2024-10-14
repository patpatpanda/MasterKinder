using System;
using System.Data.SqlClient;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Function1.cs
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public Function1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _connectionString = configuration.GetConnectionString("DefaultSQLConnection");
        }


        [Function("Function1")]
        public void Run([TimerTrigger("0 */25 * * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            // Anslut till databasen och kör en enkel fråga
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var query = "SELECT 1"; // Enkel fråga för att hålla databasen aktiv
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    _logger.LogInformation("SQL Query executed successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error executing SQL query: {ex.Message}");
                }
            }
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
