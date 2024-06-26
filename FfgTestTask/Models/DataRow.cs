using System.ComponentModel.DataAnnotations;

namespace FfgTestTask.Models
{
    /// <summary>
    /// Строка с данными
    /// </summary>
    public class DataRow
    {
        [Key]
        public int Id { get; set; }

        public int Code { get; set; }

        public string Value { get; set; }
    }
}
