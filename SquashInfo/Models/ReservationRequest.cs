using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashInfo.Models
{
    public class ReservationRequest
    {
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public TimeSpan Duration { get; set; }
        public List<int> Exclude { get; set; }
        public ReservationRequest() { }
        public ReservationRequest(string from, string to, int duration, List<int> exclude)
        {
            if (DateTime.TryParse(from, out DateTime fromTime))
            {
                FromTime = fromTime;
            }

            if (DateTime.TryParse(to, out DateTime toTime))
            {
                ToTime = toTime;
            }

            Duration = new TimeSpan(0, duration, 0);

            Exclude = exclude;
        }

        public ReservationRequest(ReservationDto res)
            : this(res.From, res.To, res.Duration, res.Exclude)
        {

        }
    }
}
