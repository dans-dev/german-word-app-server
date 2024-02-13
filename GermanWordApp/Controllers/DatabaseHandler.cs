using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;

public class DatabaseHandler {
    private string serverIP = "";
    private string adminUsername = "";
    private string adminPassword = "";
    private string dbName = "";
    private string connectionString = "";
    private MySqlConnection? connection;


    public DatabaseHandler() { }

    public void LoadConfig(string configFile) {
        // Check if the configuration file exists
        if (!File.Exists(configFile)) {
            throw new FileNotFoundException("Database configuration file not found.");
        }

        try {
            // Read all lines from the configuration file
            string[] configLines = File.ReadAllLines(configFile);

            // Parse each line to extract database connection details
            foreach (string line in configLines) {
                if (line.StartsWith("Server IP:")) serverIP = line.Substring("Server IP:".Length).Trim();
                else if (line.StartsWith("MySQL Admin Username:")) adminUsername = line.Substring("MySQL Admin Username:".Length).Trim();
                else if (line.StartsWith("MySQL Admin Password:")) adminPassword = line.Substring("MySQL Admin Password:".Length).Trim();
                else if (line.StartsWith("Database Name:")) dbName = line.Substring("Database Name:".Length).Trim();
            }

            // Check if all required database configuration details are provided
            if (string.IsNullOrEmpty(serverIP) || string.IsNullOrEmpty(adminUsername) || string.IsNullOrEmpty(adminPassword) || string.IsNullOrEmpty(dbName)) {
                throw new Exception("Database configuration is incomplete.");
            }

            // Construct connection string using the parsed details
            connectionString = $"Server={serverIP};Uid={adminUsername};Pwd={adminPassword};Database={dbName};";

        } catch (Exception ex) {
            throw new Exception("Error loading database configuration: " + ex.Message);
        }
    }

    public void Connect() {
        try {
            // Create a new MySqlConnection object with the constructed connection string and open the connection
            connection = new MySqlConnection(connectionString);
            connection.Open();

        } catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public void Disconnect() {
        try {
            // Check if the connection object exists and if the connection is open, then close the connection
            if (connection != null && connection.State != ConnectionState.Closed) {
                connection.Close();
            }

        } catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public DataTable ExecuteQuery(string query) {
        DataTable dataTable = new DataTable();

        try {
            // Check if the connection object exists and if the connection is open
            if (connection != null && connection.State == ConnectionState.Open) {
                // Create a MySqlCommand and MySqlDataAdapter objects to execute the query and fill the DataTable
                using MySqlCommand command = new MySqlCommand(query, connection);
                using MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dataTable);

            } else {
                Console.WriteLine("Error: Not Connected!");
            }

        } catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
        }

        return dataTable;
    }
}
