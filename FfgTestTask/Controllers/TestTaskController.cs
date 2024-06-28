using FfgTestTask.Data;
using FfgTestTask.Exceptions;
using FfgTestTask.Models.Dtos;
using FfgTestTask.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace FfgTestTask.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestTaskController : ControllerBase
    {
        private readonly ILogger<TestTaskController> _logger;
        private readonly IDataTableService _dataTableService;

        public TestTaskController(ILogger<TestTaskController> logger, IDataTableService dataTableService)
        {
            _logger = logger;
            _dataTableService = dataTableService;
        }

        /// <summary>
        /// Сохранить данные в БД. 
        /// </summary>
        /// <param name="data">Абстрактные данные в JSON</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResult> Save([FromBody] List<Dictionary<string,string>> data)  //Dictionary используется для корректного преобразования входящего JSON
        {
            try
            {
                var newData = data.Select(dataRow => new DataRowDto
                {
                    Code = int.Parse(dataRow.First().Key),
                    Value = dataRow.First().Value
                }).ToList();

                await _dataTableService.SaveAsync(newData);

                return Results.Ok();
            }
            catch (FormatException)
            {
                return Results.BadRequest("В одной из строк в поле Code передано значение, которое не является числом");
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
        public async Task<IResult> Get(string? codeFilter = null, string? valueFilter = null) 
        {
            var codeFilters = string.IsNullOrEmpty(codeFilter)
                ? new List<int>()
                : codeFilter
                    .Split(';')
                    .Select(code => int.Parse(code))
                    .ToList();

            return Results.Ok(await _dataTableService.GetAsync(codeFilters, valueFilter));
        }
    }
}
