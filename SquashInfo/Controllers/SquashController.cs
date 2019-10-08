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

        [HttpPost("freeCourtsExclude")]
        public IActionResult GetFreeCourtsFromToExclude([FromBody] ReservationDto reservation)
        {
            if(reservation == null)
            {
                return BadRequest(reservation);
            }

            ReservationRequest res;
            try
            {
                res = new ReservationRequest(reservation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            _logger.LogInformation($"Squash Controller is reqesting Free Courts from: {res.FromTime} to: {res.ToTime} for: {res.Duration}");
            List<CourtDto> korty = _squash.GetFreeSquashCourts(res);

            if(korty.Count() > 0 && res.isLogged())
            {
                var kort = korty.FirstOrDefault();
                var freeTime = kort.Free.FirstOrDefault();
                int lenght = (res.Duration.Hours * 60 + res.Duration.Minutes) / 30;
                kort.Free.Clear();
               
                for(int i = 0; i < lenght; i++)
                {
                    var time = new FreeHoursDto() { From = freeTime.From.AddMinutes(i * 30), To = freeTime.From.AddMinutes((i * 30) + 30) };
                    kort.Free.Add(time);
                }

                var book = GenerateBookRequest(kort, res);
                var rezerwujResponse = _squash.RezerwujTest(book);

                if (rezerwujResponse != null && rezerwujResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    kort.Booked = true;
                    var restult = new List<CourtDto>();
                    restult.Add(kort);
                    return Ok(restult);
                }
            }

            _messanger.Send("### Squash Team", $"API found {korty.Count()} free squash courts ###");
            return Ok(korty);
        }

        public BookRequestDto GenerateBookRequest(CourtDto kort, ReservationRequest res)
        {
            var freeTime = new FreeHoursDto() { From = kort.Free.FirstOrDefault().From, To = kort.Free.FirstOrDefault().To };
            var bookRequest = new BookRequestDto(res);
            int lenght = (res.Duration.Hours * 60 + res.Duration.Minutes) / 30;
            string[] tmpRez = new string[lenght];
            string tmp = String.Empty;

            for(int i = 0; i < lenght; i++)
            {
                tmp = $"{kort.ObkietId}_{freeTime.From.Hour}:{freeTime.From.Minute.ToString("D2")}_";
                freeTime.From = freeTime.From.AddMinutes(30);
                tmp += $"{freeTime.From.Hour}:{freeTime.From.Minute.ToString("D2")}";

                tmpRez[i] = tmp;
            }

            bookRequest.Rez = tmpRez;
            return bookRequest;
        }

        [HttpPost("bookCourt")]
       public IActionResult BookCourt([FromBody] BookRequestDto book)
        {
            if(book == null)
            {
                return BadRequest("Request was empty");
            }

            var response = _squash.RezerwujTest(book);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response.Content);
            }
            else
            {
                return BadRequest(response.Content);
            }

            
        }

        //[HttpGet("freeCourts/{from}/{to}/{minutes}")]
        //public IActionResult GetFreeCourtsFromTo(string from, string to, string minutes)
        //{

        //    if (!DateTime.TryParse(from, out DateTime fromTime))
        //    {
        //        _logger.LogError($"System was not able to parse {from} to Start Time.");
        //        return BadRequest($"System was not able to parse {from} to Start Time.");
        //    }

        //    if (!DateTime.TryParse(to, out DateTime toTime))
        //    {
        //        _logger.LogError($"System was not able to parse {to} to End Time.");
        //        return BadRequest($"System was not able to parse {to} to End Time.");
        //    }

        //    if(!Int16.TryParse(minutes, out Int16 min))
        //    {
        //        _logger.LogError($"System was not able to parse {minutes} to reqested play time.");
        //        return BadRequest($"System was not able to parse {minutes} to reqested play time.");
        //    }
        //    TimeSpan requestedTime = new TimeSpan(0, min, 0);

        //    _logger.LogInformation($"Squash Controller is reqesting Free Courts from: {fromTime} to: {toTime} for: {requestedTime}");
        //    List<CourtDto> korty = _squash.GetFreeSquashCourts(new ReservationRequest() { FromTime= fromTime, ToTime= toTime,Duration= requestedTime });

        //    _messanger.Send("### Squash Team", $"API found {korty.Count()} free squash courts ###");
        //    return Ok(korty);
        //}

        private ReservationRequest ConvertReservation(ReservationDto reservation)
        {
            ReservationRequest request = new ReservationRequest(reservation);


            return request;
        }
    }
}
