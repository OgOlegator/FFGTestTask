using FfgTestTask.Data;
using System.IO;

namespace FfgTestTask.Services.DbLogger
{
    public class DataBaseLoggerProvider : ILoggerProvider
    {
        private readonly string _connectionString;

        public DataBaseLoggerProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DataBaseLogger(_connectionString);
        }

        public void Dispose()
        {
            //Реализация не требуется
        }
    }
}
