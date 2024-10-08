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

            if (transaction == null)
            {
                using (var connection = GetConnection())
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
            }
            else if (transaction != null && transaction.Connection != null)
            {
                var connection = transaction.Connection;
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
            }

            return result;
        }

        public T? ExecuteReader<T>(string query, Action<MySqlParameterCollection> parameters, Func<MySqlDataReader, T> mapper, MySqlTransaction? transaction = null)
        {
            T? result = default;

            if (transaction == null)
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand(query, connection))
                {
                    if (transaction != null) command.Transaction = transaction;
                    parameters?.Invoke(command.Parameters);
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        result = mapper(reader);
                    }

                }
            }
            else if (transaction != null && transaction.Connection != null)
            {
                var connection = transaction.Connection;
                using (var command = new MySqlCommand(query, connection))
                {
                    if (transaction != null) command.Transaction = transaction;
                    parameters?.Invoke(command.Parameters);
                    var reader = command.ExecuteReader();

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
            if (transaction == null)
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand(query, connection))
                {
                    if (transaction != null) command.Transaction = transaction;
                    parameters?.Invoke(command.Parameters);
                    result = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            else
            {
                if (transaction != null && transaction.Connection != null)
                {
                    var connection = transaction.Connection;
                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (transaction != null) command.Transaction = transaction;
                        parameters?.Invoke(command.Parameters);
                        result = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            return result;
        }

        public int ExecuteNonQuery(string query, Action<MySqlParameterCollection> parameters, MySqlTransaction? transaction = null)
        {
            int filasAfectadas = 0;

            if (transaction == null)
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand(query, connection))
                {
                    parameters?.Invoke(command.Parameters);
                    try
                    {
                        filasAfectadas = command.ExecuteNonQuery();
                    }
                    catch (MySqlException)
                    {

                    }

                }
            }
            else
            {
                if (transaction.Connection != null)
                {
                    using (var command = new MySqlCommand(query, transaction.Connection))
                    {
                        command.Transaction = transaction;
                        parameters?.Invoke(command.Parameters);
                        try
                        {
                            filasAfectadas = command.ExecuteNonQuery();
                        }
                        catch (MySqlException)
                        {

                        }
                    }
                }
            }

            return filasAfectadas;
        }
    }
}
