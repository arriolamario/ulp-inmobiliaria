namespace ProyectoInmobiliaria.Repositorio;

using MySql.Data.MySqlClient;

public abstract class RepositorioBase
{
    private string connectionString;
    public RepositorioBase(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
    }

    public List<t> ExecuteReader<t>(string query, Func<MySqlDataReader, t> mapper)
    {
        List<t> result = new List<t>();


        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
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



}