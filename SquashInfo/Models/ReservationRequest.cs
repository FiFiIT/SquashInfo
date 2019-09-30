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
                FromTime = startDate;
            }
            else
            {
                throw new Exception($"StartDate parameter cannot be converted: '{res.StartDate}'");
            }

            if (DateTime.TryParse(res.StartTime, out DateTime fromTime))
            {
                StartDate = fromTime;
            }

            if (DateTime.TryParse(res.EndTime, out DateTime toTime))
            {
                ToTime = toTime;
            }

            Duration = new TimeSpan(0, res.Duration, 0);

            Exclude = res.Exclude;
        }
    }
}
