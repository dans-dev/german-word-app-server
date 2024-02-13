using System.Data;

class Program {
    static void Main() {
        string configFile = "../database_config.txt";
        DatabaseHandler dbHandler = new DatabaseHandler(configFile);
        dbHandler.Connect();

        string query = "SELECT * FROM Words;";
        DataTable result = dbHandler.ExecuteQuery(query);

        Console.WriteLine("Results of the query:");
        foreach (DataRow row in result.Rows) {
            foreach (DataColumn column in result.Columns) {
                Console.Write($"{column.ColumnName}: {row[column]} | ");
            }
            Console.WriteLine();
        }

       dbHandler.Disconnect();
    }
}