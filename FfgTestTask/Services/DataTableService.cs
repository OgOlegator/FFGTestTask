using FfgTestTask.Data;
using FfgTestTask.Exceptions;
using FfgTestTask.Models;
using FfgTestTask.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace FfgTestTask.Services
{
    public class DataTableService : IDataTableService
    {
        private readonly AppDbContext _context;

        public DataTableService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение данных из БД
        /// </summary>
        /// <param name="codeFilter">Фильтр по полю Code</param>
        /// <param name="searchValue">Фильтр по полюс Value</param>
        /// <returns>Данные из БД</returns>
        public async Task<List<DataRow>> GetAsync(List<int> codeFilter, string? searchValue)
        {
            return await _context.DataTable
                .Where(dataRow
                    => (codeFilter.Count() == 0 || codeFilter.Contains(dataRow.Code))
                    && (string.IsNullOrEmpty(searchValue) || dataRow.Value.Contains(searchValue)))
                .ToListAsync();
        }

        /// <summary>
        /// Сохранение в БД
        /// </summary>
        /// <param name="data">Данные</param>
        /// <returns></returns>
        /// <exception cref="FfgTestTaskException">Ошибка при обновлении данных</exception>
        public async Task SaveAsync(List<DataRow> data)
        {
            var saveData = data
                .Select(dataRow => new DataRow
                {
                    Code = dataRow.Code,
                    Value = dataRow.Value
                })
                .OrderBy(dataRow => dataRow.Code)
                .ToList();

            try
            {
                await _context.DataTable.ExecuteDeleteAsync();

                await _context.DataTable.AddRangeAsync(saveData);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new FfgTestTaskException("Ошибка при обновлении данных");
            }
        }
    }
}
