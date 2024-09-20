using MySql.Data.MySqlClient;

namespace InmobiliariaCA.Repositorio
{
    public abstract class RepositorioBase
    {
        private readonly string connectionString;

        public RepositorioBase(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
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

            using (var connection = transaction?.Connection ?? GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                if (transaction != null) command.Transaction = transaction;
                parameters?.Invoke(command.Parameters);

                using (var reader = command.ExecuteReader())
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
            T? result = default;

            using (var connection = transaction?.Connection ?? GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                if (transaction != null) command.Transaction = transaction;

                using (var reader = command.ExecuteReader())
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
            T? result = default;

            using (var connection = transaction?.Connection ?? GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                if (transaction != null) command.Transaction = transaction;
                parameters?.Invoke(command.Parameters);

                using (var reader = command.ExecuteReader())
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
            int result = 0;

            using (var connection = transaction?.Connection ?? GetConnection())
            using (var command = new MySqlCommand(query, connection))
            {
                if (transaction != null) command.Transaction = transaction;
                parameters?.Invoke(command.Parameters);
                result = Convert.ToInt32(command.ExecuteScalar());
            }

            return result;
        }

        public int ExecuteNonQuery(string query, Action<MySqlParameterCollection> parameters, MySqlTransaction? transaction = null)
        {
            int filasAfectadas = 0;
            try
            {
                using (var connection = transaction?.Connection ?? GetConnection())
                using (var command = new MySqlCommand(query, connection))
                {
                    if (transaction != null) command.Transaction = transaction;
                    parameters?.Invoke(command.Parameters);
                    filasAfectadas = command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451) // Código de error para restricción de clave foránea
                {
                    filasAfectadas = 0;
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }


            return filasAfectadas;
        }
    }
}
