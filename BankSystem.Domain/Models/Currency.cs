using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public struct Currency
    {
        public string Name { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Code { get; set; }

        public Currency(string name, decimal exchangeRate, string code)
        {
            Name=name;
            ExchangeRate = exchangeRate;
            Code = code;
        }
    }
}
