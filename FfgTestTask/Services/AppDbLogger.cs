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

            await _context.AppLogs.AddAsync(new LogRow
            {
                DetailsJson = logMsgDetailsJson,
            });

            Thread.Sleep(10000);

            await _context.SaveChangesAsync();
        }
    }
}
