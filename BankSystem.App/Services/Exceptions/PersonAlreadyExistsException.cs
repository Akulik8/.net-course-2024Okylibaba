using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Services.Exceptions
{
    public class PersonAlreadyExistsException : Exception
    {
        public PersonAlreadyExistsException(string message) : base(message) { }
    }
}
