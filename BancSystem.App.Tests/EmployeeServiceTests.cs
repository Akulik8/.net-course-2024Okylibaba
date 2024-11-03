using AutoMapper;
using BankSystem.App.DTOs;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Tests
{
    public class EmployeeServiceTests
    {
        private readonly IEmployeeStorage _employeeStorage;
        private readonly IEmployeeService _employeeService;
        private readonly TestDataGenerator _testDataGenerator;
        private readonly IMapper _mapper;

        public EmployeeServiceTests()
        {
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseNpgsql("Host=localhost;Port=5434;Username=postgres;Password=mysecretpassword;Database=local")
                .Options;

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ClientProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _employeeStorage = new EmployeeStorage(new Data.BankSystemDbContext(options));
            _employeeService = new EmployeeService(_employeeStorage, _mapper);
            _testDataGenerator = new TestDataGenerator();
        }


        [Fact]
        public async Task AddEmployeePositiveTest()
        {
            // Arrange
            var employees = _testDataGenerator.GenerateEmployees(10);

            // Act
            foreach (var employee in employees)
            {
                await _employeeService.AddEmployeeAsync(_mapper.Map<EmployeeDto>(employee));
            }

            Employee expectedEmployee = employees[0];

            // Assert
            Assert.Contains(expectedEmployee, await _employeeStorage.GetAsync(100,1,null));
        }

        [Fact]
        public async Task AddEmployeeThrowsPersonAlreadyExistsException()
        {
            // Arrange
            var employees = _testDataGenerator.GenerateEmployees(10);

            // Act
            foreach (var employee in employees)
            {
                await _employeeService.AddEmployeeAsync(_mapper.Map<EmployeeDto>(employee));
            }

            Employee expectedEmployee = employees[0];

            // Assert
            await Assert.ThrowsAsync<PersonAlreadyExistsException>(() => _employeeService.AddEmployeeAsync(_mapper.Map<EmployeeDto>(expectedEmployee)));
        }

        [Fact]
        public async Task AddEmployeeThrowsPersonTooYoungException()
        {
            // Act
            Employee employee = new Employee
            {
                Name = "Лилиан",
                Surname = "Галатонов",
                Date = new DateOnly(2010, 1, 1)
            };

            // Assert
            await Assert.ThrowsAsync<PersonTooYoungException>(() => _employeeService.AddEmployeeAsync(_mapper.Map<EmployeeDto>(employee)));
        }

        [Fact]
        public async Task AddEmployeeThrowsNoPassportException()
        {
            // Act
            Employee employee = new Employee
            {
                Name = "Лилиан",
                Surname = "Галатонов",
                Date = new DateOnly(2000, 1, 1)
            };

            // Assert
            await Assert.ThrowsAsync<NoPassportException>(() => _employeeService.AddEmployeeAsync(_mapper.Map<EmployeeDto>(employee)));
        }

        [Fact]
        public async Task UpdateEmployeePositivTest()
        {
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

            await _employeeStorage.AddAsync(employee);

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
            await _employeeService.UpdateEmployeeAsync(employee.Id, _mapper.Map<EmployeeDto>(updatedEmployee));

            // Assert
            var employees = await _employeeStorage.GetByIdAsync(employee.Id);
            var myEmployee = employees.LastOrDefault(e => e.Id == employee.Id);

            Assert.Equal(myEmployee.Id, updatedEmployee.Id);
        }

        [Fact]
        public async Task UpdateEmployeeThrowNotFoundException()
        {
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
            await Assert.ThrowsAsync<NotFoundException>(() => _employeeService.UpdateEmployeeAsync(newEmployee.Id, _mapper.Map<EmployeeDto>(newEmployee)));
        }

        [Fact]
        public async Task GetEmployeesPositiveTest()
        {
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

            await _employeeService.AddEmployeeAsync(_mapper.Map<EmployeeDto>(employee1));
            await _employeeService.AddEmployeeAsync(_mapper.Map<EmployeeDto>(employee2));
            await _employeeService.AddEmployeeAsync(_mapper.Map<EmployeeDto>(employee3));

            // Act
            var resultByName = await _employeeService.FindEmployeeAsync(name: "Иван");
            var resultBySurname = await _employeeService.FindEmployeeAsync(surname: "Петров");
            var resultByPhone = await _employeeService.FindEmployeeAsync(phoneNumber: "1111222233");
            var resultByPassport = await _employeeService.FindEmployeeAsync(pasNumber: "2345 678901");


            // Assert 
            Assert.Equal(_mapper.Map<EmployeeDto>(employee1), resultByName);

            Assert.Equal(_mapper.Map<EmployeeDto>(employee2), resultBySurname);

            Assert.Equal(_mapper.Map<EmployeeDto>(employee3), resultByPhone);

            Assert.Equal(_mapper.Map<EmployeeDto>(employee2), resultByPassport);
        }
    }
}
