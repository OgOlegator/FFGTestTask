using FfgTestTask.Exceptions;
using FfgTestTask.Models;
using FfgTestTask.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace FfgTestTask.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestTaskController : ControllerBase
    {
        private readonly IDataTableService _dataTableService;

        public TestTaskController(IDataTableService dataTableService)
        {
            _dataTableService = dataTableService;
        }

        /// <summary>
        /// Сохранить данные в БД. 
        /// </summary>
        /// <param name="data">Абстрактные данные в JSON</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResult> SaveAsync([FromBody] List<Dictionary<string,string>> data)  //Dictionary используется для корректного преобразования входящего JSON
        {
            try
            {
                var newData = data.Select(dataRow => new DataRow
                {
                    Code = int.Parse(dataRow.First().Key),
                    Value = dataRow.First().Value
                }).ToList();

                await _dataTableService.SaveAsync(newData);

                return Results.Ok();
            }
            catch (FormatException ex)
            {
                return Results.BadRequest("One of the lines in the Code field contains a value that is not a number");
            }
            catch (FfgTestTaskException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получить данные из БД
        /// </summary>
        /// <param name="codeFilter">Ограничения по коду, можно указать несколько через ';'</param>
        /// <param name="valueFilter">Ограничения по значению, через поиск подстроки</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResult> GetAsync(string? codeFilter = null, string? valueFilter = null) 
        {
            try
            {
                var codeFilters = string.IsNullOrEmpty(codeFilter)
                    ? new List<int>()
                    : codeFilter
                        .Split(';')
                        .Select(code => int.Parse(code))
                        .ToList();

                var result = await _dataTableService.GetAsync(codeFilters, valueFilter);

                return Results.Ok(result);
            }
            catch (FormatException ex)
            {
                return Results.BadRequest("In one of the Code filters, a value that is not a number was passed");
            }
        }
    }
}
