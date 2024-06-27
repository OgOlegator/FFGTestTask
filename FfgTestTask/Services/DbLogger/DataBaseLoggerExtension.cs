using FfgTestTask.Data;

namespace FfgTestTask.Services.DbLogger
{
    public static class DataBaseLoggerExtension
    {
        /// <summary>
        /// Добавление логгера в DI
        /// </summary>
        public static ILoggingBuilder AddDataBaseLogger(this ILoggingBuilder builder, string connectionString)
        {
            builder.AddProvider(new DataBaseLoggerProvider(connectionString));
            
            return builder;
        }
    }
}
