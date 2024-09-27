using System.Security.Principal;
using BankSystem.Domain.Models;
using BankSystem.App.Services;

namespace BankSystem.App.Tests
{
    public class EquivalenceTests
    {
        [Fact]
        public void GetHashCodeNecessityPositivTest()
        {
            // Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            List<Client> clients = testDataGenerator.GenerateClients(15);
            Dictionary<Client, List<Account>> clientAccounts = testDataGenerator.GenerateClientAccounts(clients);
            var soughtClient = clientAccounts.First();

            Client matchingClient = new Client
            {
                Name = soughtClient.Key.Name,
                Surname = soughtClient.Key.Surname,
                PhoneNumber = soughtClient.Key.PhoneNumber,
                Passport = soughtClient.Key.Passport,
                Address = soughtClient.Key.Address,
                Date = soughtClient.Key.Date,
                AccountNumber = soughtClient.Key.AccountNumber,
                Balance = soughtClient.Key.Balance
            };

            // Act
            List<Account> expectedAccount = clientAccounts[soughtClient.Key];
            List<Account> actualAccount = clientAccounts[matchingClient];

            // Assert
            Assert.Equal(actualAccount, expectedAccount);
        }

        [Fact]
        public void EmployeeNecessityPositiveTest()
        {
            // Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            List<Employee> employees= testDataGenerator.GenerateEmployees(15);
            Dictionary<Employee, List<Account>> employeesAccounts = testDataGenerator.GenerateEmployeesAccounts(employees);
            var soughtEmployee = employeesAccounts.First();

            Employee matchingEmployee = new Employee
            {
                Name = soughtEmployee.Key.Name,
                Surname = soughtEmployee.Key.Surname,
                PhoneNumber = soughtEmployee.Key.PhoneNumber,
                Passport = soughtEmployee.Key.Passport,
                Address = soughtEmployee.Key.Address,
                Date = soughtEmployee.Key.Date,
                Position = soughtEmployee.Key.Position,
                Salary = soughtEmployee.Key.Salary,
                DateStartWork = soughtEmployee.Key.DateStartWork,
                Contract = soughtEmployee.Key.Contract
            };

            // Act
            List<Account> expectedAccount = employeesAccounts[soughtEmployee.Key];
            List<Account> actualAccount = employeesAccounts[matchingEmployee];

            // Assert
            Assert.Equal(actualAccount, expectedAccount);
        }
    }
}