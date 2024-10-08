using BankSystem.App.Interfaces;
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
            IClientStorage storage = new ClientStorage();
            var clientService = new ClientService(storage);
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

                storage.Add(client);
                foreach (var account in accounts)
                    storage.AddAccount(client, account);
            }

            Client expectedClient = clients[0];

            // Assert
            Assert.Contains(expectedClient, storage.Get(null).Keys);
        }
    }
}