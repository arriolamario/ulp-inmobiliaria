namespace InmobiliariaCA.Repositorio;

using MySql.Data.MySqlClient;

public abstract class RepositorioBase
{
    private string connectionString;
    public RepositorioBase(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
    }

    public List<T> ExecuteReaderList<T>(string query, Action<MySqlParameterCollection> parameters, Func<MySqlDataReader, T> mapper)
    {
        List<T> result = new List<T>();


        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                parameters(command.Parameters);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(mapper(reader));
                    }
                }
            }
        }
        return result;
    }

    public T? ExecuteReader<T>(string query, Func<MySqlDataReader, T> mapper)
    {
        T? result = default(T);

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = mapper(reader);
                    }
                }
            }
        }

        return result;
    }

    public T? ExecuteReader<T>(string query, Action<MySqlParameterCollection> parameters, Func<MySqlDataReader, T> mapper)
    {
        T? result = default(T);

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                parameters(command.Parameters);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = mapper(reader);
                    }
                }
            }
        }

        return result;
    }

    public int ExecuteScalar(string query, Action<MySqlParameterCollection> parameters)
    {
        int result = default(int);

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                parameters(command.Parameters);
                result = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        return result;
    }

    public int ExecuteNonQuery(string query, Action<MySqlParameterCollection> parameters)
    {
        int filasAfectadas = default(int);

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                parameters(command.Parameters);
                filasAfectadas = Convert.ToInt32(command.ExecuteNonQuery());
            }
        }

        return filasAfectadas;
    }

}