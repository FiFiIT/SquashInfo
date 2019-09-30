using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashInfo.Models
{
    public class ReservationRequest
    {
        private DateTime StartDate { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public TimeSpan Duration { get; set; }
        public List<int> Exclude { get; set; }
        public ReservationRequest() { }

        public ReservationRequest(ReservationDto res)
        {
            if (DateTime.TryParse(res.StartDate, out DateTime startDate))
            {
                StartDate = startDate;
            }
            else
            {
                throw new Exception($"StartDate parameter cannot be converted: '{res.StartDate}'");
            }

            if (TimeSpan.TryParse(res.StartTime, out TimeSpan fromTime))
            {
                FromTime = StartDate.Add(fromTime);
            }
            else
            {
                throw new Exception($"StartTime parameter cannot be converted: '{res.StartTime}'");
            }

            if (TimeSpan.TryParse(res.EndTime, out TimeSpan toTime))
            {
                ToTime = StartDate.Add(toTime);
            }
            else
            {
                throw new Exception($"EndTime parameter cannot be converted: '{res.EndTime}'");
            }


            Duration = new TimeSpan(0, res.Duration, 0);

            Exclude = res.Exclude;
        }
    }
}
