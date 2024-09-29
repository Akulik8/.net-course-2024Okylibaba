using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Employee : Person
    {
        public string Position { get; set; }
        public int Salary { get; set; }
        public DateOnly DateStartWork { get; set; }
        public string Contract { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Employee))
                return false;

            var employee = (Employee)obj;
            return employee.Name == Name &&
                employee.Surname == Surname &&
                employee.PhoneNumber == PhoneNumber &&
                employee.Passport == Passport &&
                employee.Address == Address &&
                employee.Date == Date &&
                employee.Position == Position &&
                employee.Salary == Salary &&
                employee.DateStartWork == DateStartWork &&
                employee.Contract == Contract;

        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() + Surname.GetHashCode() + PhoneNumber.GetHashCode() + Passport.GetHashCode() + Address.GetHashCode() + Date.GetHashCode() + Position.GetHashCode() + Salary.GetHashCode() + DateStartWork.GetHashCode() + Contract.GetHashCode();
        }
    }
}
