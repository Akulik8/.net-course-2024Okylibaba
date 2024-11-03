using AutoMapper;
using BankSystem.App.DTOs;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;
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
        private readonly IClientStorage _clientStorage;
        private readonly IClientService _clientService;
        private readonly TestDataGenerator _testDataGenerator;
        private readonly IMapper _mapper;

        public ClientServiceTests()
        {
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseNpgsql("Host=localhost;Port=5434;Username=postgres;Password=mysecretpassword;Database=local")
                .Options;

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ClientProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _clientStorage = new ClientStorage(new Data.BankSystemDbContext(options));
            _clientService = new ClientService(_clientStorage, _mapper);
            _testDataGenerator = new TestDataGenerator();
        }

        [Fact]
        public async Task AddClientPositivTest()
        {
            // Arrange
            var clients = _testDataGenerator.GenerateClients(10);

            // Act
            foreach (ClientDto client in clients.Select(_mapper.Map<ClientDto>).ToList())
            {
               await _clientService.AddClientAsync(client);
            }

            ClientDto expectedClient = _mapper.Map<ClientDto>(clients[0]);

            var storedClients = await _clientStorage.GetAsync(1000, 1, null);

            // Assert
            Assert.Contains(expectedClient.PasNumber, storedClients.Select(_mapper.Map<ClientDto>).Select(x => x.PasNumber));
        }

        [Fact]
        public async Task AddClientThrowsPersonAlreadyExistsException()
        {
            // Arrange
            var clients = _testDataGenerator.GenerateClients(10);

            // Act
            foreach (ClientDto client in clients.Select(_mapper.Map<ClientDto>).ToList())
            {
                await _clientService.AddClientAsync(client);
            }

            ClientDto expectedClient = _mapper.Map<ClientDto>(clients[0]);

            // Assert
            await Assert.ThrowsAsync<PersonAlreadyExistsException>(() => _clientService.AddClientAsync(expectedClient));
        }

        [Fact]
        public async Task AddClientThrowsPersonTooYoungException()
        {
            // Act
            ClientDto client = new ClientDto
            {
                FullName = "Лилиан Галатонов",
                Date = new DateOnly(2010, 1, 1)
            };

            // Assert
            await Assert.ThrowsAsync<PersonTooYoungException>(() => _clientService.AddClientAsync(client));
        }

        [Fact]
        public async Task AddClientThrowsNoPassportException()
        {
            // Act
            ClientDto client = new ClientDto
            {
                FullName = "Лилиан Галатонов",
                Date = new DateOnly(2003, 1, 1)
            };

            // Assert
            await Assert.ThrowsAsync<NoPassportException>(() => _clientService.AddClientAsync(client));
        }

        [Fact]
        public async Task AddAccountToClientPositivTest()
        {
            // Arrange
            var clients = _testDataGenerator.GenerateClients(10);

            // Act
            foreach (Client client in clients)
            {
                await _clientService.AddClientAsync(_mapper.Map<ClientDto>(client));
            }

            var account = new Account
            {
                Amount = 0,
                //Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
                CurrencyName = "Рубль РФ"
            };

            var firstClient = clients[0];

            await _clientService.AddAccountToClientAsync(firstClient, account);
               
            var dictionaryClient = await _clientStorage.GetByIdAsync(firstClient.Id);
            List<Account> accounts = dictionaryClient[firstClient];

            // Assert
            Assert.Contains(account.CurrencyName, accounts.Select(x => x.CurrencyName));
        }

        [Fact]
        public async Task AddAccountToClientNotFoundException()
        {
            // Arrange
            var clients = _testDataGenerator.GenerateClients(10);

            // Act
            foreach (ClientDto client in clients.Select(_mapper.Map<ClientDto>).ToList())
            {
                await _clientService.AddClientAsync(client);
            }

            var account = new Account
            {
                Amount = 0,
                // Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
                CurrencyName = "Рубль РФ"
            };

            ClientDto firstClient = _mapper.Map<ClientDto>(clients[0]);
            await _clientService.AddClientAsync(firstClient);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _clientService.AddAccountToClientAsync(clients[0], account));
        }

        [Fact]
        public async Task DebitingMoneyFromAccountPositivTest()
        {
            // Arrange
            ConcurrentDictionary<Client, List<Account>> _cliensDictionary = new ConcurrentDictionary<Client, List<Account>>();
            var cancellationtTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            // Act
            await Task.Run(async () =>
            {
                while (!cancellationtTokenSource.Token.IsCancellationRequested)
                {
                    var clients = _testDataGenerator.GenerateClients(10);

                    foreach (var client in clients)
                    {
                        await _clientService.AddClientAsync(_mapper.Map<ClientDto>(client));

                        foreach (var account in client.Accounts)
                        {
                            if (account.CurrencyName == "Доллар США")
                            {
                                account.Amount = 50;
                                await _clientService.EditAccountAsync(account);
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
                tasks.Add(Task.Run(() => _clientService.DebitingMoneyFromAccount(client, client.Accounts.FirstOrDefault(), cashToDebit)));
            }

            await Task.WhenAll(tasks);

            foreach (var client in _cliensDictionary)
            {
                var clientWithAccount = await _clientService.GetAsync(client.Key.Id);
                var updatedAccount = clientWithAccount.Values.FirstOrDefault();
                Assert.Equal(40, updatedAccount.FirstOrDefault().Amount);
            }
        }

        [Fact]
        public async Task EditAccountPositivTest()
        {
            // Arrange
            var clients = _testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                await _clientStorage.AddAsync(client);
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

            await _clientService.AddAccountToClientAsync(firstClient, oldAccount);
            var newAccount = new Account
            {
                Id = oldAccount.Id,
                ClientId = firstClient.Id,
                Amount = 0,
                //  Currency = new Currency { Name = "Рубль РФ", Code = "RUB", ExchangeRate = 0.013m }
                CurrencyName = "Рубль РФ"
            };

            await _clientService.EditAccountAsync(newAccount);

            var newClient = await _clientStorage.GetByIdAsync(firstClient.Id);
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

            await _clientService.AddClientAsync(_mapper.Map<ClientDto>(client1));
            await _clientService.AddClientAsync(_mapper.Map<ClientDto>(client2));
            await _clientService.AddClientAsync(_mapper.Map<ClientDto>(client3));

            // Act
            var resultByName = await _clientStorage.GetAsync(100,1,с => с.Name == "Иван");
            var resultBySurname = await _clientStorage.GetAsync(100, 1, с => с.Surname == "Петров");
            var resultByPhone = await _clientStorage.GetAsync(100, 1, с => с.PhoneNumber == "1111222233");
            var resultByPassport = await _clientStorage.GetAsync(100, 1, с => с.Passport == "2345 678901");
            var resultByDateRange = await _clientStorage.GetAsync(100, 1, с => с.Date >= new DateOnly(1980, 1, 1) && с.Date <= new DateOnly(1995, 12, 31));


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
