using FfgTestTask.Models;

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
        void Save(List<DataRow> data);

        /// <summary>
        /// Получить данные
        /// </summary>
        /// <param name="codeFilter">Фильтр по полю Code</param>
        /// <param name="searchValue">Искомое значение</param>
        /// <returns>Данные</returns>
        List<DataRow> Get(List<int> codeFilter, string? searchValue);
    }
}
