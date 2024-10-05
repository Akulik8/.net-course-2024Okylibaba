﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Services.Exceptions
{
    public class NoPassportException : Exception
    {
        public NoPassportException(string message) : base(message) { }
    }
}