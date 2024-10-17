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
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
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
                        CurrencyName = currency.Name,
                        Amount = faker.Finance.Amount()
                    });
                }

                storage.Add(client);
                foreach (var account in accounts)
                    storage.AddAccount(client.Id, account);
            }

            Client expectedClient = clients[0];

            // Assert
            Assert.Contains(expectedClient, storage.Get(1000,1,null));
        }

        [Fact]
        public void UpdateClientPositiveTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();

            // Act
            Client client = new Client
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "3333",
                Date = new DateOnly(2000, 1, 1),
                Passport = "33333333333",
                Address = "-----",
            };

            storage.Add(client);

            var updatedClient = new Client
            {
                Id = client.Id,
                Name = "Ivan",
                Surname = client.Surname,
                PhoneNumber = client.PhoneNumber,
                Date = client.Date,
                Passport = client.Passport,
                Address = client.Address
            };

            storage.Update(client.Id, updatedClient);
            var dictionaryClient = storage.GetById(client.Id);
            
            // Assert
            Assert.Equal(dictionaryClient.Keys.FirstOrDefault(c => c.Id == updatedClient.Id), updatedClient);
        }

        [Fact]
        public void DeleteClientPositiveTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();

            // Act
            Client client = new Client
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "22222",
                Date = new DateOnly(2000, 1, 1),
                Passport = "2222222",
                Address = "-----",
            };

            storage.Add(client);
            storage.Delete(client.Id);
            var dictionaryClient = storage.GetById(client.Id);

            // Assert
            Assert.NotEqual(dictionaryClient.Keys.FirstOrDefault(c => c.Id == client.Id), client);
        }

        [Fact]
        public void AddAccountPositiveTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();

            // Act
            Client client = new Client
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "1234536",
                Date = new DateOnly(2000, 1, 1),
                Passport = "11111111",
                Address = "-----",
            };

            storage.Add(client);

            var account = new Account
            {
                Amount = 1000,
                CurrencyName = "Рубль РФ"
            };

            storage.AddAccount(client.Id, account);

            var dictionaryClient = storage.GetById(client.Id);
            var accounts = dictionaryClient.Values;
            var newAccount = accounts.LastOrDefault();

            // Assert
            Assert.Contains(newAccount, a => a.Id == account.Id);
        }

        [Fact]
        public void UpdateAccountPositiveTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();

            // Act
            Client client = new Client
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "444444",
                Date = new DateOnly(2000, 1, 1),
                Passport = "44444444444",
                Address = "-----",
            };

            storage.Add(client);

            var oldAccount = new Account { Id = new Guid(), ClientId = client.Id, Amount = 1000, CurrencyName = "Евро" };
            storage.AddAccount(client.Id, oldAccount);
            var newAccount = new Account { Id = oldAccount.Id, ClientId = client.Id, Amount = 2000, CurrencyName = "Рубль РФ" };

            storage.UpdateAccount(newAccount);

            var newClient = storage.GetById(client.Id);
            var accounts = newClient.Values;
            var updatedAccount = accounts.FirstOrDefault();
            var myAccount = updatedAccount.First(a => a.Id.Equals(newAccount.Id));

            // Assert
            Assert.Equal(myAccount.Id, newAccount.Id);
        }


        [Fact]
        public void DeleteAccountPositiveTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();

            // Act
            Client client = new Client
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "5555555",
                Date = new DateOnly(2000, 1, 1),
                Passport = "5555555555",
                Address = "-----",
            };

            storage.Add(client);

            var account = new Account { Id = new Guid(), ClientId = client.Id, Amount = 1000, CurrencyName = "Евро" };

            storage.AddAccount(client.Id, account);

            storage.DeleteAccount(account.Id);

            var newClient = storage.GetById(client.Id);
            var accounts = newClient.Values;
            var updatedAccount = accounts.FirstOrDefault();
            
            // Assert
            Assert.DoesNotContain(updatedAccount, a => a.Id == account.Id);
        }

        [Fact]
        public void GetClientsByParametersWithPaginationTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();

            // Act
            Client client = new Client
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "666666",
                Date = new DateOnly(2000, 1, 1),
                Passport = "6666666666",
                Address = "-----",
            };

            var client2 = new Client
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "1212112121",
                Date = new DateOnly(2000, 1, 1),
                Passport = "12121212121212",
                Address = "-----",
            };

            var client3 = new Client
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "1313131313",
                Date = new DateOnly(2000, 1, 1),
                Passport = "13131313131",
                Address = "-----",
            };

            storage.Add(client);
            storage.Add(client2);
            storage.Add(client3);

            var count = storage.Get(10, 1,x => x.Name == "Gleb").Count;

            // Assert
            Assert.Equal(3, count);
        }
    }
}