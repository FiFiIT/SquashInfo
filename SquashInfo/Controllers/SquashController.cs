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
        private readonly ISquashService _squash;

        public SquashController(ILogger<SquashController> logger, IMessanger messanger, ISquashService squash)
        {
            _logger = logger;
            _messanger = messanger;
            _squash = squash;
        }

        [HttpGet("freeCourts")]
        public JsonResult GetCourts()
        {
            return new JsonResult(SquashDataStore.Current.FreeCourts);
        }

        [HttpGet("freeCourts/{from}/{to}/{minutes}")]
        public IActionResult GetFreeCourtsFromTo(string from, string to, string minutes)
        {

            if (!DateTime.TryParse(from, out DateTime fromTime))
            {
                _logger.LogInformation($"System was not able to parse {from} to Start Time.");
                return BadRequest($"System was not able to parse {from} to Start Time.");
            }

            if (!DateTime.TryParse(to, out DateTime toTime))
            {
                _logger.LogInformation($"System was not able to parse {to} to End Time.");
                return BadRequest($"System was not able to parse {to} to End Time.");
            }

            if(!Int16.TryParse(minutes, out Int16 min))
            {
                _logger.LogInformation($"System was not able to parse {minutes} to reqested play time.");
                return BadRequest($"System was not able to parse {minutes} to reqested play time.");
            }
            TimeSpan requestedTime = new TimeSpan(0, min, 0);

            string response = _squash.GetSquashCourst(fromTime, toTime).Result;
            var FreeCourts = _squash.ConvertSquashResponse(response);

            //var FreeCourts = SquashDataStore.Current.FreeCourts;

            var result = FreeCourts.SelectMany(c => c.Free, (court, freeHours) => new { court, freeHours })
                .Where(courtAndHours => courtAndHours.freeHours.From >= fromTime && courtAndHours.freeHours.To <= toTime)
                .Select(courtAndHours =>
                    new 
                    {
                        Number = courtAndHours.court.Number,
                        Free = new FreeHoursDto()
                        {
                            From = courtAndHours.freeHours.From,
                            To = courtAndHours.freeHours.To
                        }
                    }
                ).GroupBy(c => c.Number,
                    c => c.Free,
                    (groupKey, Free) => new
                    {
                        Number = groupKey,
                        Free
                    });

            if (result == null)
            {
                return NotFound();
            }

            List<CourtDto> korty = new List<CourtDto>();
            CourtDto curCourt = null;

            foreach (var kort in result)
            {
                curCourt = new CourtDto() { Number = kort.Number, Free = new List<FreeHoursDto>() };
                FreeHoursDto prevTime = null;

                foreach (var curTime in kort.Free)
                {
                    if (prevTime is null)
                    {
                        prevTime = curTime;
                    }
                    else
                    {
                        if (curTime.From == prevTime.To)
                        {
                            prevTime.To = curTime.To;
                        }
                        else
                        {
                            if (prevTime.AvailableTime >= requestedTime)
                            {
                                curCourt.Free.Add(prevTime);
                            }

                            prevTime = curTime;
                        }
                    }
                }

                if (prevTime.AvailableTime >= requestedTime)
                {
                    curCourt.Free.Add(prevTime);
                }

                if(curCourt.Free.Count() > 0)
                {
                    korty.Add(curCourt);
                }
            }



            _messanger.Send("### Squash Team", $"API found {korty.Count()} free squash courts ###");
            return Ok(korty);
        }
    }
}
