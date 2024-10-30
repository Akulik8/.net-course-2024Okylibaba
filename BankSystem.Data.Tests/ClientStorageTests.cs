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
        public async Task AddClientPositiveTest()
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

                await storage.AddAsync(client);
                foreach (var account in accounts)
                   await storage.AddAccountAsync(client.Id, account);
            }

            Client expectedClient = clients[0];

            // Assert
            Assert.Contains(expectedClient, await storage.GetAsync(1000,1,null));
        }

        [Fact]
        public async Task UpdateClientPositiveTest()
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

            await storage.AddAsync(client);

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

            await storage.UpdateAsync(client.Id, updatedClient);
            var dictionaryClient = await storage.GetByIdAsync(client.Id);
            
            // Assert
            Assert.Equal(dictionaryClient.Keys.FirstOrDefault(c => c.Id == updatedClient.Id), updatedClient);
        }

        [Fact]
        public async Task DeleteClientPositiveTest()
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

            await storage.AddAsync(client);
            await storage.DeleteAsync(client.Id);
            var dictionaryClient = await storage.GetByIdAsync(client.Id);

            // Assert
            Assert.NotEqual(dictionaryClient.Keys.FirstOrDefault(c => c.Id == client.Id), client);
        }

        [Fact]
        public async Task AddAccountPositiveTest()
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

            await storage.AddAsync(client);

            var account = new Account
            {
                Amount = 1000,
                CurrencyName = "Рубль РФ"
            };

            await storage.AddAccountAsync(client.Id, account);

            var dictionaryClient = await storage.GetByIdAsync(client.Id);
            var accounts = dictionaryClient.Values;
            var newAccount = accounts.LastOrDefault();

            // Assert
            Assert.Contains(newAccount, a => a.Id == account.Id);
        }

        [Fact]
        public async Task UpdateAccountPositiveTest()
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

            await storage.AddAsync(client);

            var oldAccount = new Account { Id = new Guid(), ClientId = client.Id, Amount = 1000, CurrencyName = "Евро" };
            await storage.AddAccountAsync(client.Id, oldAccount);
            var newAccount = new Account { Id = oldAccount.Id, ClientId = client.Id, Amount = 2000, CurrencyName = "Рубль РФ" };

            await storage.UpdateAccountAsync(newAccount);

            var newClient = await storage.GetByIdAsync(client.Id);
            var accounts = newClient.Values;
            var updatedAccount = accounts.FirstOrDefault();
            var myAccount = updatedAccount.First(a => a.Id.Equals(newAccount.Id));

            // Assert
            Assert.Equal(myAccount.Id, newAccount.Id);
        }


        [Fact]
        public async Task DeleteAccountPositiveTest()
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

            await storage.AddAsync(client);

            var account = new Account { Id = new Guid(), ClientId = client.Id, Amount = 1000, CurrencyName = "Евро" };

            await storage.AddAccountAsync(client.Id, account);

            await storage.DeleteAccountAsync(account.Id);

            var newClient = await storage.GetByIdAsync(client.Id);
            var accounts = newClient.Values;
            var updatedAccount = accounts.FirstOrDefault();
            
            // Assert
            Assert.DoesNotContain(updatedAccount, a => a.Id == account.Id);
        }

        [Fact]
        public async Task GetClientsByParametersWithPaginationTest()
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

            await storage.AddAsync(client);
            await storage.AddAsync(client2);
            await storage.AddAsync(client3);

            var listClient = await storage.GetAsync(10, 1,x => x.Name == "Gleb");
            var count = listClient.Count();

            // Assert
            Assert.Equal(3, count);
        }
    }
}