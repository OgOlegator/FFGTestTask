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

        public async Task LogAsync(string message, Dictionary<string, object> parameters = null)
        {
            var logMsgDetailsJson = JsonSerializer.Serialize(new
            {
                message,
                parameters
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
