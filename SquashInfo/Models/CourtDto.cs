using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashInfo.Models
{
    public class CourtDto
    {
        public int Number { get; set; }
        public int ObkietId { get; set; }
        public IList<FreeHoursDto> Free { get; set; }
    }
}
