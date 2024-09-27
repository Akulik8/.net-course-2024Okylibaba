using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace BankSystem.Domain.Models
{
    public class Client : Person
    {
        public int AccountNumber { get; set; }
        public decimal Balance { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Client))
                return false;

            var client = (Client)obj;
            return client.Name == Name &&
                client.Surname == Surname &&
                client.PhoneNumber == PhoneNumber &&
                client.Passport == Passport &&
                client.Address == Address &&
                client.Date == Date &&
                client.AccountNumber == AccountNumber &&
                client.Balance == Balance;

        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() + Surname.GetHashCode() + PhoneNumber.GetHashCode() + Passport.GetHashCode() + Address.GetHashCode() + Date.GetHashCode() + AccountNumber.GetHashCode() + AccountNumber.GetHashCode() + Balance.GetHashCode();
        }
    }
}
