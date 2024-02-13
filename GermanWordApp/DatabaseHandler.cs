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

    public DatabaseHandler(string configFile) {
        LoadConfig(configFile);
    }

    private void LoadConfig(string configFile) {
        if (!File.Exists(configFile)) {
            throw new FileNotFoundException("Database configuration file not found.");
        }

        try {

            string[] configLines = File.ReadAllLines(configFile);

            foreach (string line in configLines) {
                if (line.StartsWith("Server IP:")) serverIP = line.Substring("Server IP:".Length).Trim();
                else if (line.StartsWith("MySQL Admin Username:")) adminUsername = line.Substring("MySQL Admin Username:".Length).Trim();
                else if (line.StartsWith("MySQL Admin Password:")) adminPassword = line.Substring("MySQL Admin Password:".Length).Trim();
                else if (line.StartsWith("Database Name:")) dbName = line.Substring("Database Name:".Length).Trim();
            }


            if (string.IsNullOrEmpty(serverIP) || string.IsNullOrEmpty(adminUsername) || string.IsNullOrEmpty(adminPassword) || string.IsNullOrEmpty(dbName)) {
                throw new Exception("Database configuration is incomplete.");
            }

            connectionString = $"Server={serverIP};Uid={adminUsername};Pwd={adminPassword};Database={dbName};";
        } catch (Exception ex) {
            throw new Exception("Error loading database configuration: " + ex.Message);
        }
    }

    public void Connect() {
        try {
            connection = new MySqlConnection(connectionString);
        } catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public void Disconnect() {
        try {
            connection = null;
        } catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public DataTable ExecuteQuery(string query) {
        DataTable dataTable = new DataTable();

        try {
            if (connection is not null) {
                connection.Open();

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                adapter.Fill(dataTable);

                connection.Close();
            } else {
                Console.WriteLine("Error: Not Connected!");
            }
        } catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
        }

        return dataTable;
    }
}
