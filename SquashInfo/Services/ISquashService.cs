using SquashInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SquashInfo.Services
{
    public interface ISquashService
    {
        //Task<string> GetSquashCourst(DateTime from, DateTime to);
        //List<CourtDto> ConvertSquashResponse(string hastaResponse);
        List<CourtDto> GetFreeSquashCourts(ReservationRequest request);
        Task<string> Rezerwuj(BookRequestDto book);
        Task<string> RezerwujPotwierdz(BookRequestDto book);
        HttpResponseMessage RezerwujTest(BookRequestDto book);
    }
}
