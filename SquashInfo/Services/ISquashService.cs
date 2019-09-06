using SquashInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashInfo.Services
{
    public interface ISquashService
    {
        Task<string> GetSquashCourst(DateTime from, DateTime to);
        List<CourtDto> ConvertSquashResponse(string hastaResponse);
    }
}
