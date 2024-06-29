using FfgTestTask.Data;
using FfgTestTask.Models;
using FfgTestTask.Services.IServices;
using Microsoft.VisualBasic;
using System.Text.Json;

namespace FfgTestTask.Services
{
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

            var logMsgDetailsJson = JsonSerializer.Serialize(new
            {
                logLevel,
                message,
                dateTime,
                parameters,
                exception,
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
