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

        public async Task<List<Employee>> GetAsync(Employee employee)
        {
            return await _employeeStorage.GetByIdAsync(employee.Id);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _employeeStorage.GetByIdAsync(employee.Id);
            if (existingEmployee.Any())
                throw new PersonAlreadyExistsException("Этот сотрудник уже есть.");

            DateTime today = DateTime.Today;
            int age = (today.Year - employee.Date.Year) - (today.DayOfYear < employee.Date.DayOfYear ? 1 : 0);
            if (age < 18)
                throw new PersonTooYoungException("Сотрудник не должен быть моложе 18 лет.");
            if (string.IsNullOrEmpty(employee.Passport))
                throw new NoPassportException("У сотрудника нет паспортных данных.");

            await _employeeStorage.AddAsync(employee);
        }

        public async Task RemoveClientAsync(Employee employee)
        {
            var existingEmployee = await _employeeStorage.GetByIdAsync(employee.Id);
            if (!existingEmployee.Any())
                throw new NotFoundException("Сотрудник не найден.");

            await _employeeStorage.DeleteAsync(employee.Id);
        }

        public async Task UpdateEmployeeAsync(Employee newEmployee)
        {
            var existingEmployee = await _employeeStorage.GetByIdAsync(newEmployee.Id);
            if (!existingEmployee.Any())
                throw new NotFoundException("Сотрудник не найден.");
            if (newEmployee == null)
                throw new Exception("Нет сведений о новом сотруднике.");

            await _employeeStorage.UpdateAsync(newEmployee.Id, newEmployee);
        }

        public async Task<List<Employee>> GetEmployeesByFilterAsync(int pageSize, int pageNumber, Expression<Func<Employee, bool>>? filter)
        {
            return await _employeeStorage.GetAsync(pageSize, pageNumber, filter);
        }
    }
}
