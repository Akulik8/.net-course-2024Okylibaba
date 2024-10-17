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
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new BankSystemDbContext());
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
            Assert.Contains(expectedEmployee, storage.Get(10,1,null));
        }

        [Fact]
        public void UpdateEmployeetPositiveTest()
        {
            // Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new BankSystemDbContext());
            var employeeService = new EmployeeService(storage);
            var testDataGenerator = new TestDataGenerator();

            // Act
            Employee employee = new Employee()
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "3333",
                Date = new DateOnly(2000, 1, 1),
                Passport = "33333333333",
                Address = "-----",
                Position = "Бухгалтер",
                Contract = "Контракт заключен",
                Salary = 20000,
                DateStartWork = new DateOnly(2020, 1, 1)
            };

            storage.Add(employee);

            Employee newEmployee = new Employee()
            {
                Id = employee.Id,
                Name = "Ivan",
                Surname = employee.Surname,
                PhoneNumber = employee.PhoneNumber,
                Date = employee.Date,
                Passport = employee.Passport,
                Address = employee.Address,
                Position = employee.Position,
                Contract = employee.Contract,
                Salary = employee.Salary,
                DateStartWork = employee.DateStartWork
            };

            storage.Update(newEmployee.Id, newEmployee);

            var employees = storage.GetById(employee.Id);
            var myEmployee = employees.FirstOrDefault(e => e.Id == employee.Id);

            Assert.Equal(myEmployee.Id, newEmployee.Id);
        }


        [Fact]
        public void DeleteEmployeePositiveTest()
        {
            // Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new BankSystemDbContext());
            var employeeService = new EmployeeService(storage);
            var testDataGenerator = new TestDataGenerator();
            var employees = testDataGenerator.GenerateEmployees(10);

            // Act
            Employee employee = new Employee()
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "3333",
                Date = new DateOnly(2000, 1, 1),
                Passport = "33333333333",
                Address = "-----",
                Position = "Бухгалтер",
                Contract = "Контракт заключен",
                Salary = 20000,
                DateStartWork = new DateOnly(2020, 1, 1)
            };

            storage.Add(employee);

            storage.Delete(employee.Id);

            var newEmployee = storage.GetById(employee.Id);
            Assert.NotEqual(newEmployee.FirstOrDefault(c => c.Id == employee.Id), employee);
        }
    }
}