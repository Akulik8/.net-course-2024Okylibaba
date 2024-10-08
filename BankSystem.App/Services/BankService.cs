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
        private List<Person> BlackList = new List<Person>();

        public void AddBonus(Person person)
        {
            person.Bonus += 1;
        }

        public void AddToBlackList<T>(T person) where T : Person
        {
            BlackList.Add(person);
        }

        public bool IsPersonInBlackList<T>(T person) where T : Person
        {
            return BlackList.Contains(person);
        }

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
