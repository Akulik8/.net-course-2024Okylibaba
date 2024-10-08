using BankSystem.App.Interfaces;
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
            IEmployeeStorage storage = new EmployeeStorage();
            var employeeService = new EmployeeService(storage);
            var testDataGenerator = new TestDataGenerator();
            var employees = testDataGenerator.GenerateEmployees(10);

            // Act
            foreach (var employee in employees)
            {
                storage.Add(employee);
            }

            Employee expectedEmployee = employees[0];

            // Assert
            Assert.Contains(expectedEmployee, storage.Get(null));
        }
    }
}