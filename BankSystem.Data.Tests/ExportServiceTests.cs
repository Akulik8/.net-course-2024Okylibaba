using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
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
            var exportService = new ExportService(@"E:\Practic\.net-course-2024Okylibaba\", "WriteClients.csv");

            //Act
            exportService.WriteClientsToCsv(clients);
            var readClients = exportService.ReadClientsFromCsv();

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
            var exportService = new ExportService(@"E:\Practic\.net-course-2024Okylibaba\", "ReadClients.csv");

            //Act
            exportService.WriteClientsToCsv(clients);
            var readClients = exportService.ReadClientsFromCsv();

            foreach (var client in readClients) 
            {
                storage.Add(client);
            }

            //Assert
            Assert.NotEmpty(readClients);
        }
    }
}
