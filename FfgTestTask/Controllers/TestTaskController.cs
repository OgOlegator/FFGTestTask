using Microsoft.AspNetCore.Mvc;

namespace FfgTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestTaskController : ControllerBase
    {
        [HttpPost(Name = "SaveData")]
        public IResult Save([FromBody] Dictionary<string, string> data)
        {


            return Results.Ok();
        }

        [HttpGet(Name = "GetData")]
        public IResult Get() 
        { 
            return Results.Ok();
        }
    }
}
