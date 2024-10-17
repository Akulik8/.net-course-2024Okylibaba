using BankSystem.App.Interfaces;
using BankSystem.App.Services.Exceptions;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Services
{
    public class EmployeeService
    {
        private readonly IStorage<Employee, List<Employee>> _employeeStorage;

        public EmployeeService(IStorage<Employee, List<Employee>> employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public List<Employee> Get(Employee employee)
        {
            return _employeeStorage.GetById(employee.Id);
        }

        public void AddEmployee(Employee employee)
        {
            if (_employeeStorage.GetById(employee.Id).Any())
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
            if (!_employeeStorage.GetById(employee.Id).Any())
                throw new NotFoundException("Сотрудник не найден.");

            _employeeStorage.Delete(employee.Id);
        }

        public void UpdateEmployee(Employee newEmployee)
        {
            if (!_employeeStorage.GetById(newEmployee.Id).Any())
                throw new NotFoundException("Сотрудник не найден.");
            if (newEmployee == null)
                throw new Exception("Нет сведений о новом сотруднике.");

            _employeeStorage.Update(newEmployee.Id, newEmployee);
        }

        public List<Employee> GetEmployeesByFilter(int pageSize, int pageNumber, Func<Employee, bool>? filter)
        {
            return _employeeStorage.Get(pageSize, pageNumber, filter);
        }
    }
}
