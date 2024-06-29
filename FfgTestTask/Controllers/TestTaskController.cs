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
        private readonly ILogger<TestTaskController> _logger;
        private readonly IAppLogger _appLogger;
        private readonly IDataTableService _dataTableService;

        public TestTaskController(ILogger<TestTaskController> logger, IDataTableService dataTableService, IAppLogger appLogger)
        {
            _logger = logger;
            _dataTableService = dataTableService;
            _appLogger = appLogger;
        }

        /// <summary>
        /// Сохранить данные в БД. 
        /// </summary>
        /// <param name="data">Абстрактные данные в JSON</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResult> SaveAsync([FromBody] List<Dictionary<string,string>> data)  //Dictionary используется для корректного преобразования входящего JSON
        {
            var responseId = GetResonseId(); //Для удобства отслеживания выполнения запроса в логе
            var methodName = "SaveAsync";

            await _appLogger.LogAsync(
                LogLevel.Information, 
                $"Start executing {methodName}",
                new Dictionary<string, object>
                {
                    { "ResponseId", responseId},
                    { "Data", data },
                });

            try
            {
                var newData = data.Select(dataRow => new DataRow
                {
                    Code = int.Parse(dataRow.First().Key),
                    Value = dataRow.First().Value
                }).ToList();

                await _dataTableService.SaveAsync(newData);

                await _appLogger.LogAsync(
                    LogLevel.Information,
                    $"Success executing {methodName}",
                    new Dictionary<string, object>
                    {
                        { "ResponseId", responseId},
                    });

                return Results.Ok();
            }
            catch (FormatException)
            {
                var errMessage = "В одной из строк в поле Code передано значение, которое не является числом";
                await _appLogger.LogAsync(
                    LogLevel.Error,
                    $"Error executing {methodName}",
                    new Dictionary<string, object>
                    {
                        { "ResponseId", responseId},
                        { "ErrorMessage", errMessage}
                    });

                return Results.BadRequest(errMessage);
            }
            catch (FfgTestTaskException ex)
            {
                await _appLogger.LogAsync(
                    LogLevel.Error,
                    $"Error executing {methodName}",
                    new Dictionary<string, object>
                    {
                        { "ResponseId", responseId},
                        { "ErrorMessage", ex.Message}
                    });

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
            var responseId = GetResonseId(); //Для удобства отслеживания выполнения запроса в логе
            var methodName = "GetAsync";

            await _appLogger.LogAsync(
                LogLevel.Information,
                $"Start executing {methodName}",
                new Dictionary<string, object>
                {
                    { "ResponseId", responseId},
                    { "CodeFilter", codeFilter },
                    { "ValueFilter", valueFilter },
                });

            var codeFilters = string.IsNullOrEmpty(codeFilter)
                ? new List<int>()
                : codeFilter
                    .Split(';')
                    .Select(code => int.Parse(code))
                    .ToList();

            var result = await _dataTableService.GetAsync(codeFilters, valueFilter);

            await _appLogger.LogAsync(
                LogLevel.Information,
                $"Success executing {methodName}",
                new Dictionary<string, object>
                {
                    { "ResponseId", responseId},
                    { "Result", result },
                });

            return Results.Ok(result);
        }

        private string GetResonseId()
            => Guid.NewGuid().ToString();
    }
}
