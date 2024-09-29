using BankSystem.Domain.Models;
using Bogus;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Currency = BankSystem.Domain.Models.Currency;


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
                .RuleFor(e => e.DateStartWork, f => DateOnly.FromDateTime(f.Date.Past(10)))
                .FinishWith((f, e) =>
                {
                     e.Contract = $"Контракт для {e.Surname} {e.Name}, Должность: {e.Position}, Зарплата: {e.Salary} руб., Дата начала работы: {e.DateStartWork}";
                });

            return employeeFaker.Generate(count);
        }

        public Dictionary<Client, List<Account>> GenerateClientAccounts(List<Client> clients)
        {
            Random random = new Random();
            Faker faker = new Faker("ru");

            return clients.ToDictionary(client => client, client =>
            {
                var availableCurrencies = new List<Currency>
                {
                    new Currency { Name = "Доллар США", Code = "USD", ExchangeRate = 1.0m },
                    new Currency { Name = "Евро", Code = "EUR", ExchangeRate = 1.2m },
                    new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
                };

                int accountCount = random.Next(1, Math.Min(availableCurrencies.Count, 4));
                var accounts = new List<Account>();

                for (int i = 0; i < accountCount; i++)
                {
                    var currency = availableCurrencies[random.Next(availableCurrencies.Count)];
                    availableCurrencies.Remove(currency);

                    accounts.Add(new Account
                    {
                        Currency = currency,
                        Amount = faker.Finance.Amount()
                    });
                }

                return accounts;
            });
        }

        public Dictionary<Employee, List<Account>> GenerateEmployeesAccounts(List<Employee> employees)
        {
            Random random = new Random();
            Faker faker = new Faker("ru");

            return employees.ToDictionary(employees => employees, employees =>
            {
                var availableCurrencies = new List<Currency>
                {
                    new Currency { Name = "Доллар США", Code = "USD", ExchangeRate = 1.0m },
                    new Currency { Name = "Евро", Code = "EUR", ExchangeRate = 1.2m },
                    new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
                };

                int accountCount = random.Next(1, Math.Min(availableCurrencies.Count, 4));
                var accounts = new List<Account>();

                for (int i = 0; i < accountCount; i++)
                {
                    var currency = availableCurrencies[random.Next(availableCurrencies.Count)];
                    availableCurrencies.Remove(currency);

                    accounts.Add(new Account
                    {
                        Currency = currency,
                        Amount = faker.Finance.Amount()
                    });
                }

                return accounts;
            });
        }
    }
}