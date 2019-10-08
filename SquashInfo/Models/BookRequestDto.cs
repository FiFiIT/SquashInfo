using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashInfo.Models
{
    public class BookRequestDto
    {
        private ReservationRequest res;

        public BookRequestDto()
        {
        }

        public BookRequestDto(ReservationRequest res)
        {
            Login = res.Login;
            Password = res.Password;
            Data = res.StartDate;
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime Data { get; set; }
        public string[] Rez { get; set; }
    }
}