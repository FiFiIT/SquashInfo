using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SquashInfo.Services
{
    public class FakeMessanger : IMessanger
    {
        public void Send(string to, string message)
        {
            Debug.WriteLine($"Sendig to '{to}' message: {message}");
        }
    }
}
