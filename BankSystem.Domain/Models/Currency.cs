using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BankSystem.Domain.Models
{
    public struct Currency
    {
        public string Name { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Code { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Currency))
                return false;

            var currency = (Currency)obj;
            return currency.Name == Name &&
                currency.ExchangeRate == ExchangeRate &&
                currency.Code == Code;

        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, ExchangeRate, Code);
        }
    }
}
