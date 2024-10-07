using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Services.Exceptions
{
    public class PersonTooYoungException : Exception
    {
        public PersonTooYoungException(string message) : base(message) { }
    }
}
