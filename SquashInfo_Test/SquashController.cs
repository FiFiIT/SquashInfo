using NUnit.Framework;
using SquashInfo.Models;
using SquashInfo.Services;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class Tests
    {
        SquashService squashService;
        DateTime fromTime;
        DateTime toTime;
        TimeSpan duration;
        List<CourtDto> FreeCourts;

        [SetUp]
        public void Setup()
        {
            squashService = new SquashService();

            fromTime = new DateTime() + new TimeSpan(21,0,0);
            toTime = new DateTime() + new TimeSpan(24, 0, 0);
            duration = new TimeSpan(1,0,0);

            FreeCourts = new List<CourtDto>(){
                new CourtDto(){
                    Number = 1,
                    Free = new List<FreeHoursDto>()
                    {
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(15,00,0),
                            To = new DateTime() + new TimeSpan(15,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(15,30,0),
                            To = new DateTime() + new TimeSpan(16,00,0)
                        },
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(16,0,0),
                            To = new DateTime() + new TimeSpan(16,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(17,0,0),
                            To = new DateTime() + new TimeSpan(17,30,0)
                        }
                    }
                },
                new CourtDto(){
                    Number = 4,
                    Free = new List<FreeHoursDto>()
                    {
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(15,00,0),
                            To = new DateTime() + new TimeSpan(15,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(16,0,0),
                            To = new DateTime() + new TimeSpan(16,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(16,30,0),
                            To = new DateTime() + new TimeSpan(17,00,0)
                        }
                    }
                },
                new CourtDto(){
                    Number = 7,
                    Free = new List<FreeHoursDto>()
                    {
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(19,00,0),
                            To = new DateTime() + new TimeSpan(19,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(20,0,0),
                            To = new DateTime() + new TimeSpan(20,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(22,30,0),
                            To = new DateTime() + new TimeSpan(23,00,0)
                        }
                        ,
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(23,00,0),
                            To = new DateTime() + new TimeSpan(23,30,0)
                        }
                    }
                }
            };
            
        }

        [Test]
        public void GroupFreeCourts_Should_Provide_Results()
        {
            List<CourtDto> result = squashService.GroupFreeCourts(FreeCourts, fromTime, toTime, duration);

            Assert.IsNotEmpty(result);
        }
        [Test]
        public void GroupFreeCourts_Should_Find_1_Court()
        {
            List<CourtDto> result = squashService.GroupFreeCourts(FreeCourts, fromTime, toTime, duration);

            Assert.AreEqual(7, result[0].Number);
        }
    }
}