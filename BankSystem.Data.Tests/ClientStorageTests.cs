using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Tests
{
    public class ClientStorageTests
    {
        [Fact]
        public void AddClientPositiveTest()
        {
            // Arrange
            var storage = new ClientStorage();
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);
            
            // Act
            foreach (var client in clients)
            {
                storage.AddClient(client);
            }

            Client expectedClient = clients[0];

            // Assert
            Assert.Contains(expectedClient, storage.GetAllClients());
        }

        [Fact]
        public void GetYoungestClientPositiveTest()
        {
            // Arrange
            var storage = new ClientStorage();
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                storage.AddClient(client);
            }

            Client expectedYoungest = clients.OrderBy(c => c.Date).LastOrDefault();

            // Assert
            Assert.Equal(expectedYoungest, storage.GetYoungestClient());
        }

        [Fact]
        public void GetOldestClientPositiveTest()
        {
            // Arrange
            var storage = new ClientStorage();
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);

            // Act
            foreach (var client in clients)
            {
                storage.AddClient(client);
            }

            Client expectedOldest = clients.OrderBy(c => c.Date).FirstOrDefault();

            // Assert
            Assert.Equal(expectedOldest, storage.GetOldestClient());
        }

        [Fact]
        public void GetAverageAgePositiveTest()
        {
            // Arrange
            var storage = new ClientStorage();
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(10);
            DateTime today = DateTime.Today;

            // Act
            foreach (var client in clients)
            {
                storage.AddClient(client);
            }

            double expectedAverageAge = clients.Average(c => (today.Year - c.Date.Year) - (today.DayOfYear < c.Date.DayOfYear ? 1 : 0));

            // Assert
            Assert.Equal(expectedAverageAge, storage.GetAverageAge());
            Console.WriteLine("");
        }
    }
}