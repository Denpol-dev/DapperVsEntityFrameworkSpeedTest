using DapperVsEntityFrameworkSpeedTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DapperVsEntityFrameworkSpeedTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FillController(IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("ef")]
        public async Task<IActionResult> AddUsersEntityFramework()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            if (await UserModel.AddUsersEntityFramework())
            {
                stopwatch.Stop();
                return Ok("Время выполнения: " + stopwatch.ElapsedMilliseconds);
            }
            return BadRequest();
        }

        [HttpPost("dapper")]
        public async Task<IActionResult> AddUsersDapper()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            if (await UserModel.AddUsersDapper(_configuration.GetConnectionString("test")))
            {
                stopwatch.Stop();
                return Ok("Время выполнения: " + stopwatch.ElapsedMilliseconds);
            }
            return BadRequest();
        }
    }
}
