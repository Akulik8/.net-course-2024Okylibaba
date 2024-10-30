using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Tests
{
    public class EmployeeServiceTests
    {
        [Fact]
        public async Task AddEmployeePositiveTest()
        {
            // Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new Data.BankSystemDbContext());
            var employeeService = new EmployeeService(storage);
            var testDataGenerator = new TestDataGenerator();
            var employees = testDataGenerator.GenerateEmployees(10);

            // Act
            foreach (var employee in employees)
            {
                await employeeService.AddEmployeeAsync(employee);
            }

            Employee expectedEmployee = employees[0];

            // Assert
            Assert.Contains(expectedEmployee, await storage.GetAsync(100,1,null));
        }

        [Fact]
        public async Task AddEmployeeThrowsPersonAlreadyExistsException()
        {
            // Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new Data.BankSystemDbContext());
            var employeeService = new EmployeeService(storage);
            var testDataGenerator = new TestDataGenerator();
            var employees = testDataGenerator.GenerateEmployees(10);

            // Act
            foreach (var employee in employees)
            {
                await employeeService.AddEmployeeAsync(employee);
            }

            Employee expectedEmployee = employees[0];

            // Assert
            await Assert.ThrowsAsync<PersonAlreadyExistsException>(() => employeeService.AddEmployeeAsync(expectedEmployee));
        }

        [Fact]
        public async Task AddEmployeeThrowsPersonTooYoungException()
        {
            // Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new Data.BankSystemDbContext());
            var employeeService = new EmployeeService(storage);

            // Act
            Employee employee = new Employee
            {
                Name = "Лилиан",
                Surname = "Галатонов",
                Date = new DateOnly(2010, 1, 1)
            };

            // Assert
            await Assert.ThrowsAsync<PersonTooYoungException>(() => employeeService.AddEmployeeAsync(employee));
        }

        [Fact]
        public async Task AddEmployeeThrowsNoPassportException()
        {
            // Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new Data.BankSystemDbContext());
            var employeeService = new EmployeeService(storage);

            // Act
            Employee employee = new Employee
            {
                Name = "Лилиан",
                Surname = "Галатонов",
                Date = new DateOnly(2000, 1, 1)
            };

            // Assert
            await Assert.ThrowsAsync<NoPassportException>(() => employeeService.AddEmployeeAsync(employee));
        }

        [Fact]
        public async Task UpdateEmployeePositivTest()
        {
            // Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new Data.BankSystemDbContext());
            var employeeService = new EmployeeService(storage);

            var employee = new Employee
            {
                Id = new Guid(),
                Name = "Gleb",
                Surname = "Ivanov",
                PhoneNumber = "333143",
                Date = new DateOnly(2000, 1, 1),
                Passport = "3333423333333",
                Address = "-----",
                Position = "Бухгалтер",
                Contract = "Контракт заключен",
                Salary = 20000,
                DateStartWork = new DateOnly(2020, 1, 1)
            };

            await storage.AddAsync(employee);

            var updatedEmployee = new Employee()
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

            // Act
            await employeeService.UpdateEmployeeAsync(updatedEmployee);

            // Assert
            var employees = await storage.GetByIdAsync(employee.Id);
            var myEmployee = employees.LastOrDefault(e => e.Id == employee.Id);

            Assert.Equal(myEmployee.Id, updatedEmployee.Id);
        }

        [Fact]
        public async Task UpdateEmployeeThrowNotFoundException()
        {
            // Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new Data.BankSystemDbContext());
            var employeeService = new EmployeeService(storage);

            // Act
            var newEmployee = new Employee
            {
                Name = "Сергей",
                Surname = "Сергеев",
                PhoneNumber = "987654321",
                Passport = "1234567890",
                Date = new DateOnly(1990, 1, 1),
                Contract = "Новый контракт",
                DateStartWork = new DateOnly(2023, 1, 1),
                Position = "Директор",
                Salary = 100000
            };

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => employeeService.UpdateEmployeeAsync(newEmployee));
        }

        [Fact]
        public async Task GetEmployeesPositiveTest()
        {
            // Arrange
            IStorage<Employee, List<Employee>> storage = new EmployeeStorage(new Data.BankSystemDbContext());
            var employeeService = new EmployeeService(storage);

            var employee1 = new Employee
            {
                Name = "Иван",
                Surname = "Иванов",
                PhoneNumber = "1234567890",
                Passport = "1234 567890",
                Date = new DateOnly(1990, 1, 15),
                Address = "-----",
                Position = "Бухгалтер",
                Contract = "Контракт заключен",
                Salary = 20000,
                DateStartWork = new DateOnly(2020, 1, 1)
            };

            var employee2 = new Employee
            {
                Name = "Петр",
                Surname = "Петров",
                PhoneNumber = "0987654321",
                Passport = "2345 678901",
                Date = new DateOnly(1985, 6, 25),
                Address = "-----",
                Position = "Бухгалтер",
                Contract = "Контракт заключен",
                Salary = 20000,
                DateStartWork = new DateOnly(2020, 1, 1)
            };

            var employee3 = new Employee
            {
                Name = "Сергей",
                Surname = "Сергеев",
                PhoneNumber = "1111222233",
                Passport = "3456 789012",
                Date = new DateOnly(2000, 3, 10),
                Address = "-----",
                Position = "Бухгалтер",
                Contract = "Контракт заключен",
                Salary = 20000,
                DateStartWork = new DateOnly(2020, 1, 1)
            };

            await employeeService.AddEmployeeAsync(employee1);
            await employeeService.AddEmployeeAsync(employee2);
            await employeeService.AddEmployeeAsync(employee3);

            // Act
            var resultByName = await employeeService.GetEmployeesByFilterAsync(100, 1, с => с.Name == "Иван");
            var resultBySurname = await employeeService.GetEmployeesByFilterAsync(100, 1, с => с.Surname == "Петров");
            var resultByPhone = await employeeService.GetEmployeesByFilterAsync(100, 1, с => с.PhoneNumber == "1111222233");
            var resultByPassport = await employeeService.GetEmployeesByFilterAsync(100, 1, с => с.Passport == "2345 678901");
            var resultByDateRange = await employeeService.GetEmployeesByFilterAsync(100, 1, с => с.Date >= new DateOnly(1980, 1, 1) && с.Date <= new DateOnly(1995, 12, 31));


            // Assert 
            Assert.Single(resultByName);
            Assert.Contains(employee1, resultByName);

            Assert.Single(resultBySurname);
            Assert.Contains(employee2, resultBySurname);

            Assert.Single(resultByPhone);
            Assert.Contains(employee3, resultByPhone);

            Assert.Single(resultByPassport);
            Assert.Contains(employee2, resultByPassport);

            Assert.Equal(2, resultByDateRange.Count);
            Assert.Contains(employee1, resultByDateRange);
            Assert.Contains(employee2, resultByDateRange);
        }
    }
}
