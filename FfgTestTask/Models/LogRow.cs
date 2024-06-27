using FfgTestTask.Enums;
using Microsoft.AspNetCore.Http.Connections;
using System.ComponentModel.DataAnnotations;

namespace FfgTestTask.Models
{
    /// <summary>
    /// Запись лога
    /// </summary>
    public class LogRow
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Детали сообщения в формате JSON
        /// </summary>
        public string DetailsJson { get; set; }
    }
}
