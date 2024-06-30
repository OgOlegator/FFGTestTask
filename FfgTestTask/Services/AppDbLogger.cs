using FfgTestTask.Data;
using FfgTestTask.Models;
using FfgTestTask.Services.IServices;
using Microsoft.VisualBasic;
using System.Text.Json;

namespace FfgTestTask.Services
{
    //Собственная реализация сервиса логирования обсуловлена постановкой задачи - "Информация о запросах и ответов методов контроллера должна
    //логгироваться в БД."
    //Лог сохраняется в БД в формате Json для универсальности ведения информации в логе
    //Отказ от реализации стандартного интерфейса ILogger обусловлен тем что в лог должна заводиться информация только о запросах и ответах методов контроллера.
    //При встраивании реализации ILogger в контейнер логирования ASP.NET будет записываться информация о других процессах работы приложения,
    //которая в БД, согласно условиям задачи, не нужна.

    /// <summary>
    /// Логер в БД
    /// </summary>
    public class AppDbLogger : IAppLogger
    {
        private readonly AppDbContext _context;
        private object _lock = new object();

        public AppDbLogger(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(LogLevel logLevel, string message, Dictionary<string, object> parameters = null, Exception exception = null)
        {
            var dateTime = DateTime.Now;

            var exceptionDetails = exception == null ? null : new
            {
                exception.GetType().Name,
                exception.Message,
            };

            var logMsgDetailsJson = JsonSerializer.Serialize(new
            {
                logLevel,
                message,
                dateTime,
                parameters,
                exceptionDetails
            });

            await Task.Run(() =>
            {
                lock (_lock)
                {
                    _context.AppLogs.Add(new LogRow
                    {
                        DetailsJson = logMsgDetailsJson,
                    });

                    _context.SaveChanges();
                }
            });
        }
    }
}
