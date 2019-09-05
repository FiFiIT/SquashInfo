using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SquashInfo.Models;
using SquashInfo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashInfo.Controllers
{
    [Route("squash")]
    public class SquashController : Controller
    {
        private readonly ILogger<SquashController> _logger;
        private readonly IMessanger _messanger;

        public SquashController(ILogger<SquashController> logger, IMessanger messanger)
        {
            _logger = logger;
            _messanger = messanger;
        }

        [HttpGet("freeCourts")]
        public JsonResult GetCourts()
        {
            return new JsonResult(SquashDataStore.Current.FreeCourts);
        }

        [HttpGet("freeCourts/{from}/{to}")]
        public IActionResult GetFreeCourtsFromTo(string from, string to)
        {

            if(!DateTime.TryParse(from, out DateTime fromTime))
            {
                _logger.LogInformation($"System was not able to parse {from} to DateTime.");
                return BadRequest($"System was not able to parse {from} to DateTime.");
            }

            if (!DateTime.TryParse(to, out DateTime toTime))
            {
                _logger.LogInformation($"System was not able to parse {to} to DateTime.");
                return BadRequest($"System was not able to parse {to} to DateTime.");
            }

            //var result = SquashDataStore.Current.FreeCourts.SelectMany(c => c.Free).Where(h => h.From >= fromTime && h.To <= toTime);
            //https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.selectmany?view=netframework-4.8
            var result = SquashDataStore.Current.FreeCourts.SelectMany(c => c.Free, (court, freeHours) => new { court, freeHours })
                .Where(courtAndHours => courtAndHours.freeHours.From >= fromTime && courtAndHours.freeHours.To <= toTime)
                .Select(courtAndHours =>
                    new
                    {
                        Number = courtAndHours.court,
                        FreeFrom = courtAndHours.freeHours.From,
                        FreeTo = courtAndHours.freeHours.To
                    }
                );

            if (result == null)
            {
                return NotFound();
            }

            _messanger.Send("### Squash Team", $"API found {result.Count()} free squash courts ###");
            return Ok(result);
        }
    }
}
