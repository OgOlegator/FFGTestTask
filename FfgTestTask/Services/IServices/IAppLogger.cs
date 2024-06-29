namespace FfgTestTask.Services.IServices
{
    /// <summary>
    /// Объект логирования 
    /// </summary>
    public interface IAppLogger
    {
        /// <summary>
        /// Добавление записи в лог
        /// </summary>
        /// <param name="logLevel">Уровень</param>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Параметры</param>
        /// <param name="exception">Объект исключения</param>
        /// <returns></returns>
        Task LogAsync(LogLevel logLevel, string message, Dictionary<string, object> parameters = null, Exception exception = null);
    }
}
