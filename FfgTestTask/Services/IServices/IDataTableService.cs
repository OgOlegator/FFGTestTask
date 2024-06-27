using FfgTestTask.Models.Dtos;
using System.Data;

namespace FfgTestTask.Services.IServices
{
    /// <summary>
    /// Сервис работы с сущностью DataTable
    /// </summary>
    public interface IDataTableService
    {
        /// <summary>
        /// Сохранить данные
        /// </summary>
        /// <param name="data">Данные</param>
        /// <returns></returns>
        Task SaveAsync(List<DataRowDto> data);

        /// <summary>
        /// Получить данные
        /// </summary>
        /// <param name="codeFilter">Фильтр по полю Code</param>
        /// <param name="searchValue">Искомое значение</param>
        /// <returns>Данные</returns>
        Task<List<DataRow>> GetAsync(List<int>? codeFilter, string? searchValue);
    }
}
