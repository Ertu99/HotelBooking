using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpPost("fire")]
        public IActionResult FireAndForget()
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("🔥 Fire-and-forget job executed!"));
            return Ok("Job queued.");
        }

        [HttpPost("delayed")]
        public IActionResult Delayed()
        {
            BackgroundJob.Schedule(() => Console.WriteLine("🕐 Delayed job executed!"), TimeSpan.FromSeconds(15));
            return Ok("Job scheduled for 15 seconds later.");
        }

        [HttpPost("recurring")]
        public IActionResult Recurring()
        {
            RecurringJob.AddOrUpdate("daily-job", () => Console.WriteLine("📅 Daily recurring job executed!"), Cron.Daily);
            return Ok("Recurring job added (runs daily).");
        }
    }
}
