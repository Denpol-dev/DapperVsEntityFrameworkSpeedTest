using DapperVsEntityFrameworkSpeedTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DapperVsEntityFrameworkSpeedTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetController(IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpGet("ef")]
        public async Task<IActionResult> GetUsersEntityFramework()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = await UserModel.GetUsersEntityFramework();
            stopwatch.Stop();
            return Ok(
                "Количество строк пользователей: " + result.Item1 + 
                "Количество строк клиентов: " + result.Item2 + 
                "Время выполнения: " + stopwatch.ElapsedMilliseconds);
        }

        [HttpGet("dapper")]
        public async Task<IActionResult> GetUsersDapper()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = await UserModel.GetUsersDapper(_configuration.GetConnectionString("test"));
            stopwatch.Stop();
            return Ok(
                "Количество строк пользователей: " + result.Item1 +
                "Количество строк клиентов: " + result.Item2 +
                "Время выполнения: " + stopwatch.ElapsedMilliseconds);
        }
    }
}
