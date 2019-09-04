using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashInfo.Controllers
{
    [Route("squash")]
    public class SquashController : Controller
    {
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
                return BadRequest($"System was not able to parse {from} to DateTime.");
            }

            if (!DateTime.TryParse(to, out DateTime toTime))
            {
                return BadRequest($"System was not able to parse {to} to DateTime.");
            }

            var result = SquashDataStore.Current.FreeCourts.Where(c => c.From >= fromTime && c.To <= toTime);
            if(result == null || result.Count() == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
