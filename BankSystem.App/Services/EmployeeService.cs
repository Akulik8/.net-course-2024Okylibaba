using BankSystem.App.Interfaces;
using BankSystem.App.Services.Exceptions;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeStorage _employeeStorage;

        public EmployeeService(IEmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public void AddEmployee(Employee employee)
        {
            if (_employeeStorage.Get(e => e.Passport == employee.Passport).Any())
                throw new PersonAlreadyExistsException("Этот сотрудник уже есть.");

            DateTime today = DateTime.Today;
            int age = (today.Year - employee.Date.Year) - (today.DayOfYear < employee.Date.DayOfYear ? 1 : 0);
            if (age < 18)
                throw new PersonTooYoungException("Сотрудник не должен быть моложе 18 лет.");
            if (string.IsNullOrEmpty(employee.Passport))
                throw new NoPassportException("У сотрудника нет паспортных данных.");

            _employeeStorage.Add(employee);
        }

        public void RemoveClient(Employee employee)
        {
            if (!_employeeStorage.Get(e => e.Passport == employee.Passport).Any())
                throw new NotFoundException("Клиент не найден.");

            _employeeStorage.Delete(employee);
        }

        public void UpdateEmployee(Employee oldEmployee, Employee newEmployee)
        {
            if (!_employeeStorage.Get(c => c.Passport == oldEmployee.Passport).Any())
                throw new NotFoundException("Сотрудник не найден.");
            if (newEmployee == null)
                throw new Exception("Нет сведений о новом сотруднике.");

            _employeeStorage.Update(oldEmployee, newEmployee);
        }

        public List<Employee> GetEmployeesByFilter(string? name, string? surname, string? phoneNumber, string? passport, DateOnly? startDate, DateOnly? endDate)
        {
            return _employeeStorage.Get(c =>
              (string.IsNullOrEmpty(name) || c.Name == name) &&
              (string.IsNullOrEmpty(surname) || c.Surname == surname) &&
              (string.IsNullOrEmpty(phoneNumber) || c.PhoneNumber == phoneNumber) &&
              (string.IsNullOrEmpty(passport) || c.Passport == passport) &&
              (!startDate.HasValue || c.Date >= startDate.Value) &&
              (!endDate.HasValue || c.Date <= endDate.Value));
        }
    }
}
