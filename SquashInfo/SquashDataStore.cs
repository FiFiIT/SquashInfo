using SquashInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashInfo
{
    public class SquashDataStore
    {
        public static SquashDataStore Current { get; } = new SquashDataStore();
        public List<CourtDto> FreeCourts { get; set; }

        public SquashDataStore()
        {
            FreeCourts = new List<CourtDto>
            {
                new CourtDto(){Number=1, From = new DateTime(2019,9,4,19,0,0), To= new DateTime(2019,9,4,19,30,0)},
                new CourtDto(){Number=1, From = new DateTime(2019,9,4,19,30,0), To= new DateTime(2019,9,4,20,00,0)},
                new CourtDto(){Number=3, From = new DateTime(2019,9,4,17,0,0), To= new DateTime(2019,9,4,17,30,0)},
                new CourtDto(){Number=3, From = new DateTime(2019,9,4,17,30,0), To= new DateTime(2019,9,4,18,00,0)},
                new CourtDto(){Number=10, From = new DateTime(2019,9,4,16,0,0), To= new DateTime(2019,9,4,16,30,0)},
                new CourtDto(){Number=10, From = new DateTime(2019,9,4,17,0,0), To= new DateTime(2019,9,4,17,30,0)}
            };
        }
    }
}
