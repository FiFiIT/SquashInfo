using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashInfo.Services
{
    public interface IMessanger
    {
        void Send(string to, string message);
    }
}
