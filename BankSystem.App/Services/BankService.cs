using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class BankService
    {
        public int CalculateSalaryOwners(int profit, int expenses, List<Employee> owners)
        {
            if (owners.Count == 0) 
            {
                throw new ArgumentException("У банка нет владельцев!");
            }

            int SalaryOwners = (profit-expenses)/owners.Count;
            return SalaryOwners;
        }

        public Employee ConvertClientToEmployee(Client client, string position, int salary) 
        {
            return new Employee
            {
                Name = client.Name,
                Surname = client.Surname,
                PhoneNumber = client.PhoneNumber,
                Passport = client.Passport,
                Address = client.Address,
                Date = client.Date,
                Position = position,
                Salary = salary,
                DateStartWork = DateOnly.FromDateTime(DateTime.Now),
            };
        }
    }
}
