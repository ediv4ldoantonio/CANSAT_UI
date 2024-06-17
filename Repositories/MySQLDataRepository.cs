using CANSAT_UI.Models;
using MySql.Data.MySqlClient;


namespace CANSAT_UI.Repositories;

public class MySQLDataRepository : IDataRepository
{
    private const string CONNECTION_STRING = "Server=localhost;Database=consumo;Uid=dev;Pwd=dev.pass123;";

    public async Task Create(Data data)
    {
        using var connection = new MySqlConnection(CONNECTION_STRING);
        await connection.OpenAsync();

        string query = $"INSERT INTO dados (corrente, potencia, energia, carga, timestamp) " +
                       "VALUES (@corrente, @potencia, @energia, @carga, @timestamp)";

        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@corrente", data.Current);
        command.Parameters.AddWithValue("@potencia", data.Power);
        command.Parameters.AddWithValue("@energia", data.Energy);
        command.Parameters.AddWithValue("@carga", data.Charge);
        command.Parameters.AddWithValue("@timestamp", data.Timestamp);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<Data>> Read()
    {
        List<Data> dataList = [];
        using (var connection = new MySqlConnection(CONNECTION_STRING))
        {
            await connection.OpenAsync();
            string query = "SELECT * FROM dados";

            using MySqlCommand command = new(query, connection);
            using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Data data = new()
                {
                    Charge = reader["carga"].ToString()!,
                    Current = reader["corrente"].ToString()!,
                    Power = reader["potencia"].ToString()!,
                    Energy = reader["energia"].ToString()!,
                    Timestamp = Convert.ToDateTime(reader["timestamp"])
                };
                dataList.Add(data);
            }
        }
        return dataList;
    }
}
