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

        /// <summary>
        /// Объект блокировки на чтение и запись в БД
        /// </summary>
        private static ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

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
        public List<DataRow> Get(List<int> codeFilter, string? searchValue)
        {
            _rwLock.EnterReadLock();

            try
            {
                return _context.DataTable
                    .Where(dataRow
                        => (codeFilter.Count() == 0 || codeFilter.Contains(dataRow.Code))
                        && (string.IsNullOrEmpty(searchValue) || dataRow.Value.Contains(searchValue)))
                    .ToList();
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Сохранение в БД
        /// </summary>
        /// <param name="data">Данные</param>
        /// <returns></returns>
        /// <exception cref="FfgTestTaskException">Ошибка при обновлении данных</exception>
        public void Save(List<DataRow> data)
        {
            var saveData = data
                .Select(dataRow => new DataRow
                {
                    Code = dataRow.Code,
                    Value = dataRow.Value
                })
                .OrderBy(dataRow => dataRow.Code)
                .ToList();

            _rwLock.EnterWriteLock();

            try
            {
                _context.DataTable.ExecuteDelete();

                _context.DataTable.AddRange(saveData);

                _context.SaveChanges();
            }
            catch
            {
                throw new FfgTestTaskException("Ошибка при обновлении данных");
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }
    }
}
