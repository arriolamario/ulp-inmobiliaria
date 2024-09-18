using MySql.Data.MySqlClient;

namespace InmobiliariaCA.Repositorio
{
    public abstract class RepositorioBase
    {
        private string connectionString;

        public RepositorioBase(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        protected MySqlConnection GetConnection()
        {
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public MySqlTransaction BeginTransaction(MySqlConnection connection)
        {
            return connection.BeginTransaction();
        }

        public List<T> ExecuteReaderList<T>(string query, Action<MySqlParameterCollection> parameters, Func<MySqlDataReader, T> mapper, MySqlTransaction? transaction = null)
        {
            List<T> result = new List<T>();

            using (MySqlCommand command = new MySqlCommand(query, transaction?.Connection ?? GetConnection()))
            {
                command.Transaction = transaction;
                parameters?.Invoke(command.Parameters);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(mapper(reader));
                    }
                }
            }

            return result;
        }

        public T? ExecuteReader<T>(string query, Func<MySqlDataReader, T> mapper, MySqlTransaction? transaction = null)
        {
            T? result = default(T);

            using (MySqlCommand command = new MySqlCommand(query, transaction?.Connection ?? GetConnection()))
            {
                command.Transaction = transaction;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = mapper(reader);
                    }
                }
            }

            return result;
        }

        public T? ExecuteReader<T>(string query, Action<MySqlParameterCollection> parameters, Func<MySqlDataReader, T> mapper, MySqlTransaction? transaction = null)
        {
            T? result = default(T);

            using (MySqlCommand command = new MySqlCommand(query, transaction?.Connection ?? GetConnection()))
            {
                command.Transaction = transaction;
                parameters?.Invoke(command.Parameters);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = mapper(reader);
                    }
                }
            }

            return result;
        }

        public int ExecuteScalar(string query, Action<MySqlParameterCollection> parameters, MySqlTransaction? transaction = null)
        {
            int result = default(int);

            using (MySqlCommand command = new MySqlCommand(query, transaction?.Connection ?? GetConnection()))
            {
                command.Transaction = transaction;
                parameters?.Invoke(command.Parameters);
                result = Convert.ToInt32(command.ExecuteScalar());
            }

            return result;
        }

        public int ExecuteNonQuery(string query, Action<MySqlParameterCollection> parameters, MySqlTransaction? transaction = null)
        {
            int filasAfectadas = default(int);

            using (MySqlCommand command = new MySqlCommand(query, transaction?.Connection ?? GetConnection()))
            {
                command.Transaction = transaction;
                parameters?.Invoke(command.Parameters);
                filasAfectadas = command.ExecuteNonQuery();
            }

            return filasAfectadas;
        }
    }
}
