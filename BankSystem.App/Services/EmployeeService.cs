using BankSystem.App.Services.Exceptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Services
{
    public class EmployeeService
    {
        private readonly EmployeeStorage _employeeStorage;

        public EmployeeService(EmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public void AddEmployee(Employee employee)
        {
            if (_employeeStorage.GetAllEmployees().Contains(employee))
            {
                throw new PersonAlreadyExistsException("Этот сотрудник уже есть.");
            }

            DateTime today = DateTime.Today;
            int age = (today.Year - employee.Date.Year) - (today.DayOfYear < employee.Date.DayOfYear ? 1 : 0);
            if (age < 18)
            {
                throw new PersonTooYoungException("Сотрудник не должен быть моложе 18 лет.");
            }

            if (string.IsNullOrEmpty(employee.Passport))
            {
                throw new NoPassportException("У сотрудника нет паспортных данных.");
            }

            _employeeStorage.AddEmployee(employee);
        }

        public void UpdateEmployee(string passportNumber, Employee newEmployee)
        {
            var employee = _employeeStorage.GetAllEmployees().FirstOrDefault(e => e.Passport == passportNumber);

            if (employee == null)
                throw new NotFoundException("Сотрудник с таким паспортом не найден");

            employee.Name = newEmployee.Name;
            employee.Surname = newEmployee.Surname;
            employee.PhoneNumber = newEmployee.PhoneNumber;
            employee.Date = newEmployee.Date;
            employee.Passport = newEmployee.Passport;
            employee.PhoneNumber = newEmployee.PhoneNumber;
            employee.Contract = newEmployee.Contract;
            employee.DateStartWork = newEmployee.DateStartWork;
            employee.Position = newEmployee.Position;
            employee.Salary = newEmployee.Salary;
        }

        public List<Employee> GetEmployees(string name, string surname, string phoneNumber, string passport, DateOnly startDate, DateOnly endDate)
        {
            return _employeeStorage.GetAllEmployees()
                .Where(c =>
                    (string.IsNullOrEmpty(name) || c.Name == name) &&
                    (string.IsNullOrEmpty(surname) || c.Surname == surname) &&
                    (string.IsNullOrEmpty(phoneNumber) || c.PhoneNumber == phoneNumber) &&
                    (string.IsNullOrEmpty(passport) || c.Passport == passport) &&
                    c.Date >= startDate && c.Date <= endDate)
                .ToList();
        }
    }
}
