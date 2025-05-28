using CANSAT_UI.Models;
using System.Data.SQLite;
using System.Globalization;


namespace CANSAT_UI.Repositories;

public class MySQLDataRepository : IDataRepository
{
    private const string DB_FILE = "MyDatabase.sqlite";

    public MySQLDataRepository()
    {
        if (!System.IO.File.Exists(DB_FILE))
        {
            SQLiteConnection.CreateFile(DB_FILE);
            using var conn = new SQLiteConnection($"Data Source={DB_FILE};Version=3;");

            conn.Open();

            // Create table with DateTime (TEXT) and double (REAL)
            string createTable = @"
                CREATE TABLE measurements (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    recorded_at TEXT,                   
                    number INTEGER,
                    value REAL
                )";
            using var cmd = new SQLiteCommand(createTable, conn);

            cmd.ExecuteNonQuery();
        }
    }

    public async Task Create(Data data)
    {
        // Insert DateTime and double
        using var conn = new SQLiteConnection($"Data Source={DB_FILE};Version=3;");

        conn.Open();

        var insert = "INSERT INTO measurements (recorded_at, number,  value) VALUES (@time, @number, @val)";

        using var cmd = new SQLiteCommand(insert, conn);
        cmd.Parameters.AddWithValue("@time", data.Timestamp.ToString("o")); // ISO 8601 format
        cmd.Parameters.AddWithValue("@number", data.Number);
        cmd.Parameters.AddWithValue("@val", data.Level);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<Data>> Read()
    {
        List<Data> dataList = [];

        using var conn = new SQLiteConnection($"Data Source={DB_FILE};Version=3;");

        conn.Open();

        // Read back
        var select = "SELECT recorded_at, value, number FROM measurements";
        using var readCmd = new SQLiteCommand(select, conn);
        using var reader = readCmd.ExecuteReader();

        while (await reader.ReadAsync())
        {
            var dateStr = reader["recorded_at"].ToString();
            var parsedDate = DateTime.Parse(dateStr!, null, DateTimeStyles.RoundtripKind);
            var parsedValue = Convert.ToDouble(reader["value"]);
            var parsedNumber = Convert.ToInt16(reader["number"]);

            Data data = new()
            {
                Level = parsedValue,
                Timestamp = parsedDate,
                Number = parsedNumber
            };

            dataList.Add(data);
        }

        return dataList;
    }
}
