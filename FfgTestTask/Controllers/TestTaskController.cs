using FfgTestTask.Data;
using Microsoft.AspNetCore.Mvc;

namespace FfgTestTask.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestTaskController : ControllerBase
    {
        private readonly ILogger<TestTaskController> _logger;
        private readonly AppDbContext _context;

        public TestTaskController(ILogger<TestTaskController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Сохранить данные в БД
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IResult Save([FromBody] Dictionary<string, string> data) //List<KeyValuePair<string, string>> data)
        {
            return Results.Ok();
        }

        /// <summary>
        /// Получить данные из БД
        /// </summary>
        /// <param name="codeFilter">Ограничения по коду, можно указать несколько через &</param>
        /// <param name="valueFilter">Ограничения по значению, посредству поиска подстроки</param>
        /// <returns></returns>
        [HttpGet]
        public IResult Get(string codeFilter = null, string valueFilter = null) 
        { 
            return Results.Ok();
        }
    }
}
