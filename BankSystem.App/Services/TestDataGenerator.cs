using BankSystem.Domain.Models;
using Bogus;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Services
{
    public class TestDataGenerator
    {
        public List<Client> GenerateClients(int count)
        {
            var clientFaker = new Faker<Client>("ru")
                .RuleFor(c => c.Name, f => f.Name.FirstName())
                .RuleFor(c => c.Surname, f => f.Name.LastName())
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.Passport, f => $"IПР {f.UniqueIndex.ToString("000000000")}")
                .RuleFor(c => c.Address, f => $"г. Тирасполь, {f.Address.StreetName()}")
                .RuleFor(c => c.Date, f => DateOnly.FromDateTime(f.Date.Past(90, DateTime.Now.AddYears(-18)).Date))
                .RuleFor(c => c.AccountNumber, f => f.UniqueIndex + 100000)
                .RuleFor(c => c.Balance, f => f.Finance.Amount(0, 10000000));

            return clientFaker.Generate(count);
        }

        public Dictionary<string, Client> GenerateClientDictionary(List<Client> clients)
        {
            var clientDictionary = clients.ToDictionary(client => client.PhoneNumber, client => client);
            return clientDictionary;
        }

        public List<Employee> GenerateEmployees(int count)
        {
            var employeeFaker = new Faker<Employee>("ru")
                .RuleFor(e => e.Name, f => f.Name.FirstName())
                .RuleFor(e => e.Surname, f => f.Name.LastName())
                .RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(e => e.Passport, f => $"IПР {f.UniqueIndex.ToString("000000000")}")
                .RuleFor(e => e.Address, f => $"г. Тирасполь, {f.Address.StreetName()}")
                .RuleFor(e => e.Date, f => DateOnly.FromDateTime(f.Date.Past(90, DateTime.Now.AddYears(-18)).Date))
                .RuleFor(e => e.Position, f => f.Name.JobTitle())
                .RuleFor(e => e.Salary, f => f.Random.Int(20000, 100000)) 
                .RuleFor(e => e.DateStartWork, f => DateOnly.FromDateTime(f.Date.Past(10)));  

            return employeeFaker.Generate(count);
        }
    }
}