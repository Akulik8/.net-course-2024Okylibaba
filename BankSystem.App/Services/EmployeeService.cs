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
            if (_employeeStorage.FindEmployeeByPassport(employee.Passport) != null)
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

        public void UpdateEmployee(string passport, Employee newEmployee)
        {
            var employee = _employeeStorage.FindEmployeeByPassport(passport);
            if (employee == null)
                throw new NotFoundException("Сотрудник с таким паспортом не найден");
            _employeeStorage.UpdateEmployee(employee, newEmployee);
        }

        public List<Employee> GetEmployeesByFilter(string name, string surname, string phoneNumber, string passport, DateOnly startDate, DateOnly endDate)
        {
            return _employeeStorage.GetEmployeesByFilter(name, surname, phoneNumber, passport, startDate, endDate);
        }
    }
}
