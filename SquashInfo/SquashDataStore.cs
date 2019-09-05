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
                new CourtDto()
                {
                    Number = 1,
                    Free = new List<FreeHoursDto>
                    {
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,19,0,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,19,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,19,30,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,20,00,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,16,00,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,16,30,0)
                        }
                    }
                },
                new CourtDto()
                {
                    Number = 3,
                    Free = new List<FreeHoursDto>
                    {
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,20,0,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,20,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,00,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,30,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,00,0)
                        }
                    }
                },
                new CourtDto()
                {
                    Number = 13,
                    Free = new List<FreeHoursDto>
                    {
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,22,0,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,22,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,00,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,22,30,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,23,00,0)
                        }
                    }
                }
            };
        }
    }
}
