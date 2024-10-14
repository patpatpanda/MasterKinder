using System;
using System.Data.SqlClient;
using System.Threading;

class Program
{
    private static Timer _timer;

    static void Main(string[] args)
    {
        Console.WriteLine("WebJob started. Press [Enter] to stop.");

        // Starta en timer som körs var 25:e minut (1500 sekunder)
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(25));

        // Håll applikationen igång tills användaren trycker på [Enter]
        Console.ReadLine();
    }

    private static void DoWork(object state)
    {
        Console.WriteLine($"Task started at: {DateTime.Now}");

        string connectionString = "Server=tcp:yellow.database.windows.net,1433;Initial Catalog=forskolatre;User ID=sqladmin;Password=Hejsan123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                var query = "SELECT 1"; // Enkel fråga för att hålla databasen aktiv
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("SQL Query executed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing SQL query: {ex.Message}");
            }
        }
    }
}
