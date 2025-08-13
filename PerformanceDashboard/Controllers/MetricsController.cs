using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace PerformanceDashboard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController : ControllerBase
    {
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            var process = Process.GetCurrentProcess();
            var cpuUsage = GetCpuUsage();
            var memoryUsage = process.WorkingSet64 / (1024 * 1024); // MB

            return Ok(new
            {
                Message = "Application is running",
                Time = DateTime.UtcNow,
                CpuUsage = cpuUsage,
                MemoryUsage = memoryUsage
            });
        }

        private double GetCpuUsage()
        {
            using (var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            {
                cpuCounter.NextValue();
                Thread.Sleep(500); // थोड़ा wait जरूरी है accurate reading के लिए
                return Math.Round(cpuCounter.NextValue(), 2);
            }
        }
    }
}
