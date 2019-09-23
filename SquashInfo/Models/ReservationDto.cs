using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashInfo.Models
{
    public class ReservationDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Duration { get; set; }
        public List<int> Exclude { get; set; }
    }
}
