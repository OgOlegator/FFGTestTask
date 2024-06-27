using FfgTestTask.Data;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace FfgTestTask.Services.DbLogger
{
    /// <summary>
    /// Логгер для записи сообщений в БД
    /// </summary>
    public class DataBaseLogger : ILogger, IDisposable
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        private readonly string _connectionString;

        private object _lock = new object();

        public DataBaseLogger(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return this;
        }

        public void Dispose()
        {
            //Реализация не требуется
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            //В рамках данной задачи нет потребности в реализации логики доступности логера
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var logMsgDetailsJson = JsonSerializer.Serialize(new
            {
                logLevel,
                eventId,
                parameters = (state as IEnumerable<KeyValuePair<string, object>>)?.ToDictionary(i => i.Key, i => i.Value),
                message = formatter(state, exception),
                exception = exception?.GetType().Name
            });

            var sqlExpression = $"INSERT INTO AppLogs (DetailsJson) VALUES ('{logMsgDetailsJson}')";

            lock (_lock)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand(sqlExpression, connection);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
