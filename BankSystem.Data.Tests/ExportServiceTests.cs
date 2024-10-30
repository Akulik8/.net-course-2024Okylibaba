using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using ExportTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Tests
{
    public class ExportServiceTests
    {
        [Fact]
        public async Task WriteClientsToCsvPositiveTest()
        {
            //Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var clients = await storage.GetAsync(10,1,null);
            var exportService = new ExportService();

            //Act
            exportService.WriteClientsToCsv(clients, @"E:\Practic\.net-course-2024Okylibaba\", "WriteClients.csv");
            var readClients = exportService.ReadClientsFromCsv(@"E:\Practic\.net-course-2024Okylibaba\", "WriteClients.csv");

            //Asssert
            Assert.Equal(clients.Count, readClients.Count);
        }

        [Fact]
        public async Task ReadClientsFromCsvPositiveTest()
        {
            //Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(8);
            var exportService = new ExportService();

            //Act
            exportService.WriteClientsToCsv(clients, @"E:\Practic\.net-course-2024Okylibaba\", "ReadClients.csv");
            var readClients = exportService.ReadClientsFromCsv(@"E:\Practic\.net-course-2024Okylibaba\", "ReadClients.csv");

            foreach (var client in readClients) 
            {
                await storage.AddAsync(client);
            }

            //Assert
            Assert.NotEmpty(readClients);
        }

        [Fact]
        public async Task WritePersonsToFileJsonWithClientsPositiveTest()
        {
            //Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var clients = await storage.GetAsync(10, 1, null);
            var exportService = new ExportService();

            //Act
            exportService.WritePersonsToFileJson(clients, @"E:\Practic\.net-course-2024Okylibaba\", "WritePersons.json");
            var readClients = exportService.ReadPersonsFromFileJson<List<Client>>(@"E:\Practic\.net-course-2024Okylibaba\", "WritePersons.json");

            //Asssert
            Assert.Equal(clients.Count, readClients.Count);
        }

        [Fact]
        public void WritePersonsToFileJsonWithClientPositiveTest()
        {
            //Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            Client client = new Client
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "4423454444",
                Date = new DateOnly(2000, 1, 1),
                Passport = "44444444342444",
                Address = "-----",
            };
            var exportService = new ExportService();

            //Act
            exportService.WritePersonToFileJson(client, @"E:\Practic\.net-course-2024Okylibaba\", "WritePersons.json");
            var readClients = exportService.ReadPersonsFromFileJson<List<Client>>(@"E:\Practic\.net-course-2024Okylibaba\", "WritePersons.json");

            //Asssert
            Assert.Contains(client, readClients);
        }

        [Fact]
        public async Task WritePersonsToFileJsonWithEmployeesPositiveTest()
        {
            //Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new BankSystemDbContext());
            var employees = await storage.GetAsync(100, 1, null);
            var exportService = new ExportService();

            //Act
            exportService.WritePersonsToFileJson(employees, @"E:\Practic\.net-course-2024Okylibaba\", "WritePersons.json");
            var readEmployees = exportService.ReadPersonsFromFileJson<List<Employee>>(@"E:\Practic\.net-course-2024Okylibaba\", "WritePersons.json");

            //Asssert
            Assert.Equal(employees.Count, readEmployees.Count);
        }

        [Fact]
        public async Task ReadPersonsFromFileJsonWithEmployeesPositiveTest()
        {
            //Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new BankSystemDbContext());
            var testDataGenerator = new TestDataGenerator();
            var employees = testDataGenerator.GenerateEmployees(8);
            var exportService = new ExportService();

            //Act
            exportService.WritePersonsToFileJson(employees, @"E:\Practic\.net-course-2024Okylibaba\", "ReadPersons.json");
            var readEmployees = exportService.ReadPersonsFromFileJson<List<Employee>>(@"E:\Practic\.net-course-2024Okylibaba\", "ReadPersons.json");

            foreach (var employee in readEmployees)
            {
                await storage.AddAsync(employee);
            }

            //Assert
            Assert.NotEmpty(readEmployees);
        }

        [Fact]
        public async Task ReadPersonsFromFileJsonWithClientsPositiveTest()
        {
            //Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(8);
            var exportService = new ExportService();

            //Act
            exportService.WritePersonsToFileJson(clients, @"E:\Practic\.net-course-2024Okylibaba\", "ReadPersons.json");
            var readClients = exportService.ReadPersonsFromFileJson<List<Client>>(@"E:\Practic\.net-course-2024Okylibaba\", "ReadPersons.json");

            foreach (var client in readClients)
            {
                await storage.AddAsync(client);
            }

            //Assert
            Assert.NotEmpty(readClients);
        }

        [Fact]
        public async Task ReadPersonsFromFileJsonWithClientPositiveTest()
        {
            //Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var exportService = new ExportService();
            Client client = new Client
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "4444123442543",
                Date = new DateOnly(2000, 1, 1),
                Passport = "4444444443454234",
                Address = "-----",
            };

            //Act
            exportService.WritePersonToFileJson(client, @"E:\Practic\.net-course-2024Okylibaba\", "ReadPersons.json");
            var readClient = exportService.ReadPersonsFromFileJson<List<Client>>(@"E:\Practic\.net-course-2024Okylibaba\", "ReadPersons.json");
            await storage.AddAsync(readClient.First());

            //Assert
            Assert.Contains(client, readClient);
        }
    }
}
