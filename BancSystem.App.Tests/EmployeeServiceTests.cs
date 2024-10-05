﻿using BankSystem.App.Services;
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
        public void AddEmployeeThrowsPersonAlreadyExistsException()
        {
            // Arrange
            var storage = new EmployeeStorage();
            var employeeService = new EmployeeService(storage);
            var testDataGenerator = new TestDataGenerator();
            var employees = testDataGenerator.GenerateEmployees(10);

            // Act
            foreach (var employee in employees)
            {
                employeeService.AddEmployee(employee);
            }

            Employee expectedEmployee = employees[0];

            // Assert
            Assert.Throws<PersonAlreadyExistsException>(() => employeeService.AddEmployee(expectedEmployee));
        }

        [Fact]
        public void AddEmployeeThrowsPersonTooYoungException()
        {
            // Arrange
            var storage = new EmployeeStorage();
            var employeeService = new EmployeeService(storage);

            // Act
            Employee employee = new Employee
            {
                Name = "Лилиан",
                Surname = "Галатонов",
                Date = new DateOnly(2010, 1, 1)
            };

            // Assert
            Assert.Throws<PersonTooYoungException>(() => employeeService.AddEmployee(employee));
        }

        [Fact]
        public void AddEmployeeThrowsNoPassportException()
        {
            // Arrange
            var storage = new EmployeeStorage();
            var employeeService = new EmployeeService(storage); ;

            // Act
            Employee employee = new Employee
            {
                Name = "Лилиан",
                Surname = "Галатонов",
                Date = new DateOnly(2000, 1, 1)
            };

            // Assert
            Assert.Throws<NoPassportException>(() => employeeService.AddEmployee(employee));
        }

        [Fact]
        public void UpdateEmployeePositivTest()
        {
            // Arrange
            var storage = new EmployeeStorage();
            var employeeService = new EmployeeService(storage);

            // Создаем и добавляем сотрудника
            var existingEmployee = new Employee
            {
                Name = "Иван",
                Surname = "Иванов",
                PhoneNumber = "123456789",
                Passport = "1234567890",
                Date = new DateOnly(1990, 1, 1),
                Contract = "Контракт",
                DateStartWork = new DateOnly(2020, 1, 1),
                Position = "Менеджер",
                Salary = 50000
            };

            storage.AddEmployee(existingEmployee);

            var updatedEmployee = new Employee
            {
                Name = "Сергей",
                Surname = "Сергеев",
                PhoneNumber = "987654321",
                Passport = "1234567890", // тот же паспорт
                Date = new DateOnly(1985, 1, 1),
                Contract = "Новый контракт",
                DateStartWork = new DateOnly(2023, 1, 1),
                Position = "Директор",
                Salary = 100000
            };

            // Act
            employeeService.UpdateEmployee("1234567890", updatedEmployee);

            // Assert
            var updatedEmp = storage.GetAllEmployees().FirstOrDefault(e => e.Passport == "1234567890");
            Assert.NotNull(updatedEmp);
            Assert.Equal("Сергей", updatedEmp.Name);
            Assert.Equal("Сергеев", updatedEmp.Surname);
            Assert.Equal("987654321", updatedEmp.PhoneNumber);
            Assert.Equal(new DateOnly(1985, 1, 1), updatedEmp.Date);
            Assert.Equal("Новый контракт", updatedEmp.Contract);
            Assert.Equal(new DateOnly(2023, 1, 1), updatedEmp.DateStartWork);
            Assert.Equal("Директор", updatedEmp.Position);
            Assert.Equal(100000, updatedEmp.Salary);
        }

        [Fact]
        public void UpdateEmployeeThrowNotFoundException()
        {
            // Arrange
            var storage = new EmployeeStorage();
            var employeeService = new EmployeeService(storage);

            // Act
            var newEmployee = new Employee
            {
                Name = "Сергей",
                Surname = "Сергеев",
                PhoneNumber = "987654321",
                Passport = "1234567890", // паспорт, который не существует в хранилище
                Date = new DateOnly(1990, 1, 1),
                Contract = "Новый контракт",
                DateStartWork = new DateOnly(2023, 1, 1),
                Position = "Директор",
                Salary = 100000
            };

            // Assert
            Assert.Throws<NotFoundException>(() => employeeService.UpdateEmployee("1234567890", newEmployee));
        }

        [Fact]
        public void GetEmployeesPositiveTest()
        {
            // Arrange
            var storage = new EmployeeStorage();
            var employeeService = new EmployeeService(storage);

            var employee1 = new Employee
            {
                Name = "Иван",
                Surname = "Иванов",
                PhoneNumber = "1234567890",
                Passport = "1234 567890",
                Date = new DateOnly(1990, 1, 15)
            };

            var employee2 = new Employee
            {
                Name = "Петр",
                Surname = "Петров",
                PhoneNumber = "0987654321",
                Passport = "2345 678901",
                Date = new DateOnly(1985, 6, 25)
            };

            var employee3 = new Employee
            {
                Name = "Сергей",
                Surname = "Сергеев",
                PhoneNumber = "1111222233",
                Passport = "3456 789012",
                Date = new DateOnly(2000, 3, 10)
            };

            employeeService.AddEmployee(employee1);
            employeeService.AddEmployee(employee2);
            employeeService.AddEmployee(employee3);

            // Act
            var resultByName = employeeService.GetEmployees("Иван", null, null, null, DateOnly.MinValue, DateOnly.MaxValue);
            var resultBySurname = employeeService.GetEmployees(null, "Петров", null, null, DateOnly.MinValue, DateOnly.MaxValue);
            var resultByPhone = employeeService.GetEmployees(null, null, "1111222233", null, DateOnly.MinValue, DateOnly.MaxValue);
            var resultByPassport = employeeService.GetEmployees(null, null, null, "2345 678901", DateOnly.MinValue, DateOnly.MaxValue);
            var resultByDateRange = employeeService.GetEmployees(null, null, null, null, new DateOnly(1980, 1, 1), new DateOnly(1995, 12, 31));


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
