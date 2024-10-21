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
        public void WriteClientsToCsvPositiveTest()
        {
            //Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var clients = storage.Get(10,1,null);
            var exportService = new ExportService();

            //Act
            exportService.WriteClientsToCsv(clients, @"E:\Practic\.net-course-2024Okylibaba\", "WriteClients.csv");
            var readClients = exportService.ReadClientsFromCsv(@"E:\Practic\.net-course-2024Okylibaba\", "WriteClients.csv");

            //Asssert
            Assert.Equal(clients.Count, readClients.Count);
        }

        [Fact]
        public void ReadClientsFromCsvPositiveTest()
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
                storage.Add(client);
            }

            //Assert
            Assert.NotEmpty(readClients);
        }

        [Fact]
        public void WritePersonsToCsvWithClientsPositiveTest()
        {
            //Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var clients = storage.Get(10, 1, null);
            var exportService = new ExportService();

            //Act
            exportService.WritePersonsToCsv(clients, @"E:\Practic\.net-course-2024Okylibaba\", "WritePersons.json");
            var readClients = exportService.ReadPersonsToCsv<List<Client>>(@"E:\Practic\.net-course-2024Okylibaba\", "WritePersons.json");

            //Asssert
            Assert.Equal(clients.Count, readClients.Count);
        }

        [Fact]
        public void WritePersonsToCsvWithEmployeesPositiveTest()
        {
            //Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new BankSystemDbContext());
            var employees = storage.Get(10, 1, null);
            var exportService = new ExportService();

            //Act
            exportService.WritePersonsToCsv(employees, @"E:\Practic\.net-course-2024Okylibaba\", "WritePersons.json");
            var readEmployees = exportService.ReadPersonsToCsv<List<Employee>>(@"E:\Practic\.net-course-2024Okylibaba\", "WritePersons.json");

            //Asssert
            Assert.Equal(employees.Count, readEmployees.Count);
        }

        [Fact]
        public void ReadPersonsToCsvWithEmployeesPositiveTest()
        {
            //Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new BankSystemDbContext());
            var testDataGenerator = new TestDataGenerator();
            var employees = testDataGenerator.GenerateEmployees(8);
            var exportService = new ExportService();

            //Act
            exportService.WritePersonsToCsv(employees, @"E:\Practic\.net-course-2024Okylibaba\", "ReadPersons.json");
            var readEmployees = exportService.ReadPersonsToCsv<List<Employee>>(@"E:\Practic\.net-course-2024Okylibaba\", "ReadPersons.json");

            foreach (var employee in readEmployees)
            {
                storage.Add(employee);
            }

            //Assert
            Assert.NotEmpty(readEmployees);
        }

        [Fact]
        public void ReadPersonsToCsvWithClientsPositiveTest()
        {
            //Arrange
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(8);
            var exportService = new ExportService();

            //Act
            exportService.WritePersonsToCsv(clients, @"E:\Practic\.net-course-2024Okylibaba\", "ReadPersons.json");
            var readClients = exportService.ReadPersonsToCsv<List<Client>>(@"E:\Practic\.net-course-2024Okylibaba\", "ReadPersons.json");

            foreach (var client in readClients)
            {
                storage.Add(client);
            }

            //Assert
            Assert.NotEmpty(readClients);
        }
    }
}
