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
        public List<CourtDto> NewCourts { get; set; }

        public SquashDataStore()
        {
            NewCourts = new List<CourtDto>
            {
                new CourtDto()
                {
                    Number = 7,
                    Free = new List<FreeHoursDto>
                    {
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,0,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,19,00,0)
                        }
                    }
                },
                new CourtDto()
                {
                    Number = 23,
                    Free = new List<FreeHoursDto>
                    {
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,0,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,30,0)
                        }
                    }
                },
                new CourtDto()
                {
                    Number = 31,
                    Free = new List<FreeHoursDto>
                    {
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,0,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,0,0)
                        }
                    }
                }
            };

            FreeCourts = new List<CourtDto>
            {
                new CourtDto()
                {
                    Number = 1,
                    Free = new List<FreeHoursDto>
                    {
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,0,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,00,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,30,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,19,00,0)
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
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,0,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,30,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,00,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,30,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,19,00,0)
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
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,0,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,30,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,00,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,00,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,30,0),
                            To = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,19,00,0)
                        }
                    }
                }
            };
        }
    }
}
