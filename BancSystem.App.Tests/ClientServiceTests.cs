﻿using BankSystem.App.Services;
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
            var storage = new ClientStorage();
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
            Assert.Contains(expectedClient, storage.GetAllClients());
        }

        [Fact]
        public void AddClientThrowsPersonAlreadyExistsException()
        {
            // Arrange
            var storage = new ClientStorage();
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
            var storage = new ClientStorage();
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
            var storage = new ClientStorage();
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
            var storage = new ClientStorage();
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
                Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m } 
            };

            var firstClient = clients[0];

            clientService.AddAccountToClient(firstClient.Passport, account);
            var clientInStorage = storage.GetAllClients().FirstOrDefault(c => c.Key.Passport == firstClient.Passport);

            // Assert
            Assert.Contains(clientInStorage.Value, a => a.Currency.Code == "RUB");
        }

        [Fact]
        public void AddAccountToClientNotFoundException()
        {
            // Arrange
            var storage = new ClientStorage();
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
                Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
            };

            var firstClientPassport = clients[0].Passport;
            storage.RemoveClient(clients[0]);

            // Assert
            Assert.Throws<NotFoundException>(() => clientService.AddAccountToClient(firstClientPassport, account));
        }

        [Fact]
        public void EditAccountPositivTest()
        {
            // Arrange
            var storage = new ClientStorage();
            var clientService = new ClientService(storage);
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                clientService.AddClient(client);
            }

            var oldAccount = new Account
            {
                Amount = 0,
                Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.01m }
            };

            var firstClient = clients[0];
            clientService.AddAccountToClient(firstClient.Passport, oldAccount);
            var newAccount = new Account
            {
                Amount = 0,
                Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
            };
            clientService.AddAccountToClient(firstClient.Passport, newAccount);

            clientService.EditAccount(firstClient.Passport, oldAccount, newAccount);

            var clientInStorage = storage.GetAllClients().FirstOrDefault(c => c.Key.Passport == firstClient.Passport);
            Account updatedAccount = clientInStorage.Value.FirstOrDefault(a => a.Currency.Name == "Рубль РФ");
           
            // Assert
            Assert.Equal(0.013m, updatedAccount.Currency.ExchangeRate);
        }

        [Fact]
        public void GetClientsPositiveTest()
        {
            // Arrange
            var storage = new ClientStorage();
            var clientService = new ClientService(storage);
            var client1 = new Client
            {
                Name = "Иван",
                Surname = "Иванов",
                PhoneNumber = "1234567890",
                Passport = "1234 567890",
                Date = new DateOnly(1990, 1, 15)
            };

            var client2 = new Client
            {
                Name = "Петр",
                Surname = "Петров",
                PhoneNumber = "0987654321",
                Passport = "2345 678901",
                Date = new DateOnly(1985, 6, 25)
            };

            var client3 = new Client
            {
                Name = "Сергей",
                Surname = "Сергеев",
                PhoneNumber = "1111222233",
                Passport = "3456 789012",
                Date = new DateOnly(2000, 3, 10)
            };

            clientService.AddClient(client1);
            clientService.AddClient(client2);
            clientService.AddClient(client3);

            // Act
            var resultByName = clientService.GetClients("Иван", null, null, null, DateOnly.MinValue, DateOnly.MaxValue);
            var resultBySurname = clientService.GetClients(null, "Петров", null, null, DateOnly.MinValue, DateOnly.MaxValue);
            var resultByPhone = clientService.GetClients(null, null, "1111222233", null, DateOnly.MinValue, DateOnly.MaxValue);
            var resultByPassport = clientService.GetClients(null, null, null, "2345 678901", DateOnly.MinValue, DateOnly.MaxValue);
            var resultByDateRange = clientService.GetClients(null, null, null, null, new DateOnly(1980, 1, 1), new DateOnly(1995, 12, 31));


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