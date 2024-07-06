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
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Параметры</param>
        /// <returns></returns>
        Task LogAsync(string message, Dictionary<string, object> parameters = null);
    }
}
