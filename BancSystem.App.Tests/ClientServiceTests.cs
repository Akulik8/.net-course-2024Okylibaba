using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Tests
{
    public class ClientServiceTests
    {
        [Fact]
        public void AddClientPositivTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                clientService.AddClient(client);
            }

            Client expectedClient = clients[0];

            // Assert
            Assert.Contains(expectedClient, storage.Get(1000,1,null));
        }

        [Fact]
        public void AddClientThrowsPersonAlreadyExistsException()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                clientService.AddClient(client);
            }

            Client expectedClient = clients[0];

            // Assert
            Assert.Throws<PersonAlreadyExistsException>(() => clientService.AddClient(expectedClient));
        }

        [Fact]
        public void AddClientThrowsPersonTooYoungException()
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
            Assert.Throws<PersonTooYoungException>(() => clientService.AddClient(client));
        }

        [Fact]
        public void AddClientThrowsNoPassportException()
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
            Assert.Throws<NoPassportException>(() => clientService.AddClient(client));
        }

        [Fact]
        public void AddAccountToClientPositivTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                clientService.AddClient(client);
            }

            var account = new Account
            {
                Amount = 0,
                //Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
                CurrencyName = "Рубль РФ"
            };

            var firstClient = clients[0];

            clientService.AddAccountToClient(firstClient, account);
               
            var dictionaryClient = storage.GetById(firstClient.Id);
            var accounts = dictionaryClient.Values;
            var newAccount = accounts.LastOrDefault();

            // Assert
            Assert.Contains(newAccount, a => a.CurrencyName == "Рубль РФ");
        }

        [Fact]
        public void AddAccountToClientNotFoundException()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                clientService.AddClient(client);
            }

            var account = new Account
            {
                Amount = 0,
                // Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
                CurrencyName = "Рубль РФ"
            };

            Client firstClient = clients[0];
            clientService.RemoveClient(firstClient);

            // Assert
            Assert.Throws<NotFoundException>(() => clientService.AddAccountToClient(firstClient, account));
        }

        [Fact]
        public void EditAccountPositivTest()
        {
            // Arrange
            IClientStorage storage = new ClientStorage(new Data.BankSystemDbContext());
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                clientService.AddClient(client);
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
                      
            clientService.AddAccountToClient(firstClient, oldAccount);
            var newAccount = new Account
            {
                Id = oldAccount.Id,
                ClientId = firstClient.Id,
                Amount = 0,
                //  Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
                CurrencyName = "Рубль РФ"
            };
  
            clientService.EditAccount(newAccount);

            var newClient = storage.GetById(firstClient.Id);
            var accounts = newClient.Values;
            var updatedAccount = accounts.FirstOrDefault();
            var myAccount = updatedAccount.First(a => a.Id.Equals(newAccount.Id));

            // Assert
            Assert.Equal(myAccount.Id, newAccount.Id);
        }

        [Fact]
        public void GetClientsPositiveTest()
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

            clientService.AddClient(client1);
            clientService.AddClient(client2);
            clientService.AddClient(client3);

            // Act
            var resultByName = clientService.Get(100,1,с => с.Name == "Иван");
            var resultBySurname = clientService.Get(100, 1,с => с.Surname == "Петров");
            var resultByPhone = clientService.Get(100, 1, с => с.PhoneNumber == "1111222233");
            var resultByPassport = clientService.Get(100, 1, с => с.Passport == "2345 678901");
            var resultByDateRange = clientService.Get(100, 1, с => с.Date >= new DateOnly(1980, 1, 1) && с.Date <= new DateOnly(1995, 12, 31));


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
