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
        private readonly IAppLogger _appLogger;
        private readonly IDataTableService _dataTableService;

        public TestTaskController(IDataTableService dataTableService, IAppLogger appLogger)
        {
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
            //Для удобства отслеживания выполнения запроса в логе
            var responseId = GetResonseId(); 
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

                return Results.Ok();
            }
            catch (FormatException ex)
            {
                var errMessage = "One of the lines in the Code field contains a value that is not a number";

                await _appLogger.LogAsync(
                    LogLevel.Error,
                    $"Error executing {methodName}",
                    new Dictionary<string, object>
                    {
                        { "ResponseId", responseId},
                        { "ErrorMessage", errMessage}
                    }, 
                    ex);

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
                    },
                    ex);

                return Results.BadRequest(ex.Message);
            }
            finally
            {
                await _appLogger.LogAsync(
                    LogLevel.Information,
                    $"End executing {methodName}",
                    new Dictionary<string, object>
                    {
                        { "ResponseId", responseId},
                    });
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
            //Для удобства отслеживания выполнения запроса в логе
            var responseId = GetResonseId(); 
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

            List<DataRow> result = null;

            try
            {
                var codeFilters = string.IsNullOrEmpty(codeFilter)
                    ? new List<int>()
                    : codeFilter
                        .Split(';')
                        .Select(code => int.Parse(code))
                        .ToList();

                result = await _dataTableService.GetAsync(codeFilters, valueFilter);

                return Results.Ok(result);
            }
            catch (FormatException ex)
            {
                var errMessage = "In one of the Code filters, a value that is not a number was passed";

                await _appLogger.LogAsync(
                    LogLevel.Error,
                    $"Error executing {methodName}",
                    new Dictionary<string, object>
                    {
                        { "ResponseId", responseId},
                        { "ErrorMessage", errMessage}
                    },
                    ex);

                return Results.BadRequest(errMessage);
            }
            finally
            {
                await _appLogger.LogAsync(
                    LogLevel.Information,
                    $"End executing {methodName}",
                    new Dictionary<string, object>
                    {
                        { "ResponseId", responseId},
                        { "Result", result },
                    });
            }
        }

        /// <summary>
        /// Сгенерировать ИД запроса
        /// </summary>
        /// <returns>ИД запроса</returns>
        private string GetResonseId()
            => Guid.NewGuid().ToString();
    }
}
