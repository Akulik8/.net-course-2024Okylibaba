using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Tests
{
    public class EmployeeStorageTests
    {
        [Fact]
        public void AddEmployeePositiveTest()
        {
            // Arrange
            var storage = new EmployeeStorage();
            var testDataGenerator = new TestDataGenerator();
            var employees = testDataGenerator.GenerateEmployees(10);

            // Act
            foreach (var employee in employees)
            {
                storage.AddEmployee(employee);
            }

            Employee expectedEmployee = employees[0];

            // Assert
            Assert.Contains(expectedEmployee, storage.GetAllEmployees());
        }

        [Fact]
        public void GetYoungestEmployeePositiveTest()
        {
            // Arrange
            var storage = new EmployeeStorage();
            var testDataGenerator = new TestDataGenerator();
            var employees = testDataGenerator.GenerateEmployees(10);

            // Act
            foreach (var employee in employees)
            {
                storage.AddEmployee(employee);
            }

            Employee expectedYoungest = employees.OrderBy(e => e.Date).LastOrDefault();

            // Assert
            Assert.Equal(expectedYoungest, storage.GetYoungestEmployee());
        }

        [Fact]
        public void GetOldestEmployeePositiveTest()
        {
            // Arrange
            var storage = new EmployeeStorage();
            var testDataGenerator = new TestDataGenerator();
            var employees = testDataGenerator.GenerateEmployees(10);

            // Act
            foreach (var employee in employees)
            {
                storage.AddEmployee(employee);
            }

            Employee expectedOldest = employees.OrderBy(e => e.Date).FirstOrDefault();

            // Assert
            Assert.Equal(expectedOldest, storage.GetOldestEmployee());
        }

        [Fact]
        public void GetAverageAgePositiveTest()
        {
            // Arrange
            var storage = new EmployeeStorage();
            var testDataGenerator = new TestDataGenerator();
            var employees = testDataGenerator.GenerateEmployees(10);
            DateTime today = DateTime.Today;

            // Act
            foreach (var employee in employees)
            {
                storage.AddEmployee(employee);
            }

            double expectedAverageAge = employees.Average(e => (today.Year - e.Date.Year) - (today.DayOfYear < e.Date.DayOfYear ? 1 : 0));

            // Assert
            Assert.Equal(expectedAverageAge, storage.GetAverageAge());
            Console.WriteLine("");
        }
    }
}