using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SquashInfo.Controllers;
using SquashInfo.Models;
using SquashInfo.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class Tests
    {
        SquashService squashService;
        DateTime fromTime;
        DateTime toTime;
        TimeSpan duration;
        List<CourtDto> foundCourt;
        List<CourtDto> FreeCourts;
        List<CourtDto> FreeCourtsExclude;
        SquashController squashController;
        SquashController squashCtrlMock;
        CourtDto kort;
        ReservationRequest res;

        [SetUp]
        public void Setup()
        {
            foundCourt = new List<CourtDto>() {
                new CourtDto
                {
                    Number = 3,
                    ObkietId = 3,
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
                            }
                        }
                }
            };
            squashService = new SquashService();

            fromTime = new DateTime() + new TimeSpan(21,0,0);
            toTime = new DateTime() + new TimeSpan(24, 0, 0);
            duration = new TimeSpan(1,0,0);

            kort = new CourtDto()
            {
                Number = 13,
                ObkietId = 23,
                Free = new List<FreeHoursDto>()
                    {
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(19,00,0),
                            To = new DateTime() + new TimeSpan(22,30,0)
                        }
                }
            };
            
            FreeCourts = new List<CourtDto>(){
                new CourtDto(){
                    Number = 1,
                    ObkietId = 1,
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
                    ObkietId = 4,
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
                    ObkietId=17,
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
                },
                    new CourtDto(){
                        Number = 13,
                        ObkietId = 23,
                        Free = new List<FreeHoursDto>()
                        {
                            new FreeHoursDto()
                            {
                                From =new DateTime() + new TimeSpan(19,00,0),
                                To = new DateTime() + new TimeSpan(22,30,0)
                            }
                    }
                }
            };
            FreeCourtsExclude = new List<CourtDto>(){
                new CourtDto(){
                    Number = 3,
                    ObkietId = 3,
                    Free = new List<FreeHoursDto>()
                    {
                        new FreeHoursDto()
                        {
                            From = new DateTime() + new TimeSpan(15,00,0),
                            To = new DateTime() + new TimeSpan(18,30,0)
                        },
                    }
                },
                new CourtDto(){
                    Number = 4,
                    ObkietId = 4,
                    Free = new List<FreeHoursDto>()
                    {
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(15,00,0),
                            To = new DateTime() + new TimeSpan(16,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(16,0,0),
                            To = new DateTime() + new TimeSpan(18,30,0)
                        }
                    }
                },
                new CourtDto(){
                    Number = 7,
                    ObkietId=7,
                    Free = new List<FreeHoursDto>()
                    {
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(19,00,0),
                            To = new DateTime() + new TimeSpan(20,30,0)
                        },
                        new FreeHoursDto()
                        {
                            From =new DateTime() + new TimeSpan(21,0,0),
                            To = new DateTime() + new TimeSpan(22,00,0)
                        }
                    }
                },
                    new CourtDto(){
                        Number = 13,
                        ObkietId = 23,
                        Free = new List<FreeHoursDto>()
                        {
                            new FreeHoursDto()
                            {
                                From =new DateTime() + new TimeSpan(16,00,0),
                                To = new DateTime() + new TimeSpan(17,30,0)
                            }
                    }
                }
            };
            var logger = new Mock<ILogger<SquashController>>();
            var messanger = new Mock<IMessanger>();
            var squashServ = new Mock<ISquashService>();
            squashServ.Setup(s => s.GetFreeSquashCourts(It.IsAny<ReservationRequest>())).Returns(FreeCourtsExclude);
            squashServ.Setup(s => s.RezerwujTest(It.IsAny<BookRequestDto>())).Returns(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK));

            squashController = new SquashController(logger.Object, messanger.Object, squashService);
            squashCtrlMock = new SquashController(logger.Object, messanger.Object, squashServ.Object);

            res = new ReservationRequest() { Duration = new TimeSpan(1,0,0), Login="login", Password="pass", StartDate = new DateTime(2019,10,7) };
        }
        [Test]
        public void GroupFreeCourts_Should_Provide_Results()
        {
            var start = new DateTime() + new TimeSpan(15, 0, 0);
            var end = new DateTime() + new TimeSpan(23, 30, 0);

            List<CourtDto> result = squashService.GroupFreeCourts(FreeCourts, start, end, duration);

            Assert.IsNotEmpty(result);
        }
        [Test]
        public void GroupFreeCourts_Should_Find_1_Court()
        {
            List<CourtDto> result = squashService.GroupFreeCourts(FreeCourts, fromTime, toTime, duration);

            Assert.AreEqual(7, result[0].Number);
        }
        [Test]
        public void BookFirstFreeCourt_Should_Create_Proper_Rez()
        {
            DateTime expectedDate = new DateTime(2019, 10, 7);
            string[] Rez = new string[] { "23_19:00_19:30", "23_19:30_20:00" };

            var result = squashController.GenerateBookRequest(kort, res);

            Assert.AreEqual(Rez, result.Rez);
            Assert.AreEqual(expectedDate, result.Data);
        }
        [Test]
        public void Rezwruj_Shoul_Book_Specified_COurts()
        {
            string[] rez = new string[] { "4_14:00_14:30", "4_14:30_15:00" };

            var rezwrujRequest = new BookRequestDto() { Data=DateTime.Now, Login="filip.tyborowski@gmai.com", Password="Marian82.", Rez=rez };

            var response = squashService.RezerwujTest(rezwrujRequest);
        }
        [Test]
        public void GetFreeCourtsFromToExclude_Should_Return_BadRequest()
        {
            var response = squashCtrlMock.GetFreeCourtsFromToExclude(null);

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }

        [Test]
        public void GetFreeCourtsFromToExclude_Reservation_NoData()
        {
            ReservationDto res = new ReservationDto();
            var response = squashCtrlMock.GetFreeCourtsFromToExclude(res);

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }
        [Test]
        public void  GetFreeCourtsFromToExclude_Reservation_OK_NotLogged()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            ReservationDto res = new ReservationDto() { Duration=60, StartDate= currentDate, StartTime="16:00",
                EndTime="17:00", Type="squash" };
            var response = squashCtrlMock.GetFreeCourtsFromToExclude(res);
            var foundCourts = (response as OkObjectResult).Value as IList<CourtDto>;

            Assert.IsInstanceOf<OkObjectResult>(response);
            Assert.AreEqual(FreeCourtsExclude, foundCourts);
        }
        [Test]
        public void GetFreeCourtsFromToExclude_Reservation_OK_Is_Logged()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            var expectedBook = JsonConvert.SerializeObject(new List<CourtDto>() {
                new CourtDto
                {
                    Number = 3,
                    ObkietId = 3,
                    Booked = true,
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
                            }
                        }
                }
                }); ;

            ReservationDto res = new ReservationDto()
            {
                Duration = 60,
                StartDate = currentDate,
                StartTime = "15:00",
                EndTime = "17:00",
                Type = "squash",
                Email="login",
                Password="pass"
            };
            var response = squashCtrlMock.GetFreeCourtsFromToExclude(res);
            var bookedCourt = JsonConvert.SerializeObject((response as OkObjectResult).Value as List<CourtDto>);

            Assert.IsInstanceOf<OkObjectResult>(response);
            Assert.AreEqual( expectedBook, bookedCourt);
        }
    }
}