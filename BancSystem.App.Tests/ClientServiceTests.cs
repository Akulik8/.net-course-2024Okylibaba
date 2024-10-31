using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Bogus;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Tests
{
    public class ClientServiceTests
    {
        [Fact]
        public async Task AddClientPositivTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
               await clientService.AddClientAsync(client);
            }

            Client expectedClient = clients[0];

            // Assert
            Assert.Contains(expectedClient, await storage.GetAsync(1000,1,null));
        }

        [Fact]
        public async Task AddClientThrowsPersonAlreadyExistsException()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                await clientService.AddClientAsync(client);
            }

            Client expectedClient = clients[0];

            // Assert
            await Assert.ThrowsAsync<PersonAlreadyExistsException>(() => clientService.AddClientAsync(expectedClient));
        }

        [Fact]
        public async Task AddClientThrowsPersonTooYoungException()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);

            // Act
            Client client = new Client
            {
                Name = "Лилиан",
                Surname = "Галатонов",
                Date = new DateOnly(2010, 1, 1)
            };

            // Assert
            await Assert.ThrowsAsync<PersonTooYoungException>(() => clientService.AddClientAsync(client));
        }

        [Fact]
        public async Task AddClientThrowsNoPassportException()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);

            // Act
            Client client = new Client
            {
                Name = "Лилиан",
                Surname = "Галатонов",
                Date = new DateOnly(2000, 1, 1)
            };

            // Assert
            await Assert.ThrowsAsync<NoPassportException>(() => clientService.AddClientAsync(client));
        }

        [Fact]
        public async Task AddAccountToClientPositivTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                await clientService.AddClientAsync(client);
            }

            var account = new Account
            {
                Amount = 0,
                //Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
                CurrencyName = "Рубль РФ"
            };

            var firstClient = clients[0];

            await clientService.AddAccountToClientAsync(firstClient, account);
               
            var dictionaryClient = await storage.GetByIdAsync(firstClient.Id);
            var accounts = dictionaryClient.Values;
            var newAccount = accounts.LastOrDefault();

            // Assert
            Assert.Contains(newAccount, a => a.CurrencyName == "Рубль РФ");
        }

        [Fact]
        public async Task AddAccountToClientNotFoundException()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                await clientService.AddClientAsync(client);
            }

            var account = new Account
            {
                Amount = 0,
                // Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
                CurrencyName = "Рубль РФ"
            };

            Client firstClient = clients[0];
            await clientService.AddClientAsync(firstClient);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => clientService.AddAccountToClientAsync(firstClient, account));
        }

        [Fact]
        public async Task DebitingMoneyFromAccountPositivTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();
            ConcurrentDictionary<Client, List<Account>> _cliensDictionary = new ConcurrentDictionary<Client, List<Account>>();
            var cancellationtTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            // Act
            await Task.Run(async () =>
            {
                while (!cancellationtTokenSource.Token.IsCancellationRequested)
                {
                    var clients = testDataGenerator.GenerateClients(10);

                    foreach (var client in clients)
                    {
                        await clientService.AddClientAsync(client);

                        foreach (var account in client.Accounts)
                        {
                            if (account.CurrencyName == "Доллар США")
                            {
                                account.Amount = 50;
                                await clientService.EditAccountAsync(account);
                            }
                        }
                        _cliensDictionary.TryAdd(client, client.Accounts.ToList());
                    }
                }
            }, cancellationtTokenSource.Token);


            var tasks = new List<Task>();
            decimal cashToDebit = 10; 

            foreach(var client  in _cliensDictionary.Keys)
            {
                tasks.Add(Task.Run(() => clientService.DebitingMoneyFromAccount(client, client.Accounts.FirstOrDefault(), cashToDebit)));
            }

            await Task.WhenAll(tasks);

            foreach (var client in _cliensDictionary)
            {
                var clientWithAccount = await clientService.GetAsync(client.Key);
                var updatedAccount = clientWithAccount.Values.FirstOrDefault();
                Assert.Equal(40, updatedAccount.FirstOrDefault().Amount);
            }
        }

        [Fact]
        public async Task EditAccountPositivTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                await clientService.AddClientAsync(client);
            }

            Client firstClient = clients[0];

            var oldAccount = new Account
            {
                Id = new Guid(),
                ClientId = firstClient.Id,
                Amount = 0,
                // Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.01m }
                CurrencyName = "Евро"
            };

            await clientService.AddAccountToClientAsync(firstClient, oldAccount);
            var newAccount = new Account
            {
                Id = oldAccount.Id,
                ClientId = firstClient.Id,
                Amount = 0,
                //  Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
                CurrencyName = "Рубль РФ"
            };

            await clientService.EditAccountAsync(newAccount);

            var newClient = await storage.GetByIdAsync(firstClient.Id);
            var accounts = newClient.Values;
            var updatedAccount = accounts.FirstOrDefault();
            var myAccount = updatedAccount.First(a => a.Id.Equals(newAccount.Id));

            // Assert
            Assert.Equal(myAccount.Id, newAccount.Id);
        }

        [Fact]
        public async Task GetClientsPositiveTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);
            var client1 = new Client
            {
                Name = "Иван",
                Surname = "Иванов",
                PhoneNumber = "1234567890",
                Passport = "1234 567890",
                Date = new DateOnly(1990, 1, 15),
                Address = "-----"
            };

            var client2 = new Client
            {
                Name = "Петр",
                Surname = "Петров",
                PhoneNumber = "0987654321",
                Passport = "2345 678901",
                Date = new DateOnly(1985, 6, 25),
                Address = "-----"
            };

            var client3 = new Client
            {
                Name = "Сергей",
                Surname = "Сергеев",
                PhoneNumber = "1111222233",
                Passport = "3456 789012",
                Date = new DateOnly(2000, 3, 10),
                Address = "-----"
            };

            await clientService.AddClientAsync(client1);
            await clientService.AddClientAsync(client2);
            await clientService.AddClientAsync(client3);

            // Act
            var resultByName = await clientService.GetAsync(100,1,с => с.Name == "Иван");
            var resultBySurname = await clientService.GetAsync(100, 1, с => с.Surname == "Петров");
            var resultByPhone = await clientService.GetAsync(100, 1, с => с.PhoneNumber == "1111222233");
            var resultByPassport = await clientService.GetAsync(100, 1, с => с.Passport == "2345 678901");
            var resultByDateRange = await clientService.GetAsync(100, 1, с => с.Date >= new DateOnly(1980, 1, 1) && с.Date <= new DateOnly(1995, 12, 31));


            // Assert 
            Assert.Single(resultByName);
            Assert.Contains(client1, resultByName);

            Assert.Single(resultBySurname);
            Assert.Contains(client2, resultBySurname);

            Assert.Single(resultByPhone);
            Assert.Contains(client3, resultByPhone);

            Assert.Single(resultByPassport);
            Assert.Contains(client2, resultByPassport);

            Assert.Equal(2, resultByDateRange.Count);
            Assert.Contains(client1, resultByDateRange);
            Assert.Contains(client2, resultByDateRange);
        }
    }
}
