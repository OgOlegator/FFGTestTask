using System.ComponentModel.DataAnnotations;

namespace FfgTestTask.Models
{
    /// <summary>
    /// Строка с данными
    /// </summary>
    public class DataRow
    {
        /// <summary>
        /// Уникальный номер строки
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }
    }
}
