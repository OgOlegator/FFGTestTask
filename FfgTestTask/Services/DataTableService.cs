using FfgTestTask.Data;
using FfgTestTask.Models.Dtos;
using FfgTestTask.Services.IServices;
using System.Data;

namespace FfgTestTask.Services
{
    public class DataTableService : IDataTableService
    {
        private readonly AppDbContext _context;

        public DataTableService(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<DataRow>> GetAsync(List<int>? codeFilter, string? searchValue)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(List<DataRowDto> data)
        {
            throw new NotImplementedException();
        }
    }
}
