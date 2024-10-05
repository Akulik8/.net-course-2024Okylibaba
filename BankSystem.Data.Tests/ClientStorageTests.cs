using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Bogus;
using Bogus.DataSets;
using System;
using Currency = BankSystem.Domain.Models.Currency;

namespace BankSystem.Data.Tests
{
    public class ClientStorageTests
    {
        [Fact]
        public void AddClientPositiveTest()
        {
            // Arrange
            var storage = new ClientStorage();
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);
            Random random = new Random();
            Faker faker = new Faker("ru");

            // Act
            foreach (var client in clients)
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

                storage.AddClient(client, accounts);
            }

            Client expectedClient = clients[0];

            // Assert
            Assert.Contains(expectedClient, storage.GetAllClients().Keys);
        }

        [Fact]
        public void GetYoungestClientPositiveTest()
        {
            // Arrange
            var storage = new ClientStorage();
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);
            Random random = new Random();
            Faker faker = new Faker("ru");

            // Act
            foreach (var client in clients)
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

                storage.AddClient(client, accounts);
            }

            Client expectedYoungest = clients.OrderBy(c => c.Date).LastOrDefault();

            // Assert
            Assert.Equal(expectedYoungest, storage.GetYoungestClient());
        }

        [Fact]
        public void GetOldestClientPositiveTest()
        {
            // Arrange
            var storage = new ClientStorage();
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);
            Random random = new Random();
            Faker faker = new Faker("ru");

            // Act
            foreach (var client in clients)
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

                storage.AddClient(client, accounts);
            }

            Client expectedOldest = clients.OrderBy(c => c.Date).FirstOrDefault();

            // Assert
            Assert.Equal(expectedOldest, storage.GetOldestClient());
        }

        [Fact]
        public void GetAverageAgePositiveTest()
        {
            // Arrange
            var storage = new ClientStorage();
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);
            DateTime today = DateTime.Today;
            Random random = new Random();
            Faker faker = new Faker("ru");

            // Act
            foreach (var client in clients)
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

                storage.AddClient(client, accounts);
            }

            double expectedAverageAge = clients.Average(c => (today.Year - c.Date.Year) - (today.DayOfYear < c.Date.DayOfYear ? 1 : 0));

            // Assert
            Assert.Equal(expectedAverageAge, storage.GetAverageAge());
            Console.WriteLine("");
        }
    }
}