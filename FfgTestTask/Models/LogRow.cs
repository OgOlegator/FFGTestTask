using FfgTestTask.Enums;
using Microsoft.AspNetCore.Http.Connections;

namespace FfgTestTask.Models
{
    /// <summary>
    /// Запись лога
    /// </summary>
    public class LogRow
    {
        public DateTime DateTime { get; set; }

        public LogMessageType Type { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }
    }
}
