using CANSAT_UI.Models;
using MySql.Data.MySqlClient;


namespace CANSAT_UI.Repositories;

public class MySQLDataRepository : IDataRepository
{
    private const string CONNECTION_STRING = "Server=localhost;Database=cansat;Uid=dev;Pwd=dev.pass123;";

    public async Task Create(Data data)
    {
        using var connection = new MySqlConnection(CONNECTION_STRING);
        await connection.OpenAsync();
        string query = "INSERT INTO data (Location, Date, Time, Temperature, Humidity, Timestamp) " +
                       "VALUES (@Location, @Date, @Time, @Temperature, @Humidity, @Timestamp)";
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@Location", data.Location);
        command.Parameters.AddWithValue("@Date", data.Date);
        command.Parameters.AddWithValue("@Time", data.Time);
        command.Parameters.AddWithValue("@Temperature", data.Temperature);
        command.Parameters.AddWithValue("@Humidity", data.Humidity);
        command.Parameters.AddWithValue("@Timestamp", data.Timestamp);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<Data>> Read()
    {
        List<Data> dataList = [];
        using (var connection = new MySqlConnection(CONNECTION_STRING))
        {
            await connection.OpenAsync();
            string query = "SELECT * FROM data";

            using MySqlCommand command = new(query, connection);
            using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Data data = new()
                {
                    Location = reader["Location"].ToString()!,
                    Date = reader["Date"].ToString()!,
                    Time = reader["Time"].ToString()!,
                    Temperature = Convert.ToDouble(reader["Temperature"]),
                    Humidity = Convert.ToDouble(reader["Humidity"]),
                    Timestamp = Convert.ToDateTime(reader["Timestamp"])
                };
                dataList.Add(data);
            }
        }
        return dataList;
    }
}
