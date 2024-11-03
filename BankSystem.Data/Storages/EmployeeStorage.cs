using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class EmployeeStorage : IEmployeeStorage
    {
        private readonly BankSystemDbContext _bankSystemDbContext;

        public EmployeeStorage(BankSystemDbContext bankSystemDbContext)
        {
            _bankSystemDbContext = bankSystemDbContext;
        }

        public async Task AddAsync(Employee employee)
        {
            await _bankSystemDbContext.Employees.AddAsync(employee);
            await _bankSystemDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _bankSystemDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee != null)
            {
                _bankSystemDbContext.Employees.Remove(employee);
                await _bankSystemDbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Guid id, Employee newEmployee) 
        {
            var employee = await _bankSystemDbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee != null)
            {
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

                await _bankSystemDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Employee>> GetByIdAsync(Guid id)
        {
            var employee = await _bankSystemDbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == id);
            if (employee != null)
            {
                return new List<Employee> { employee };
            }

            return new List<Employee>();
        }

        public async Task<List<Employee>> GetAsync(int pageSize, int pageNumber, Expression<Func<Employee, bool>>? filter)
        {
            var query = _bankSystemDbContext.Employees.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query
                .OrderBy(x => x.Surname)
                //.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }

    public async Task<Employee> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _bankSystemDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee != null)
        {
            return employee;
        }

        return new Employee();
    }

        public async Task<List<Employee>> GetEmployeesByParametersAsync(
            string? name, string? surname, string? phoneNumber, string? pasNumber, int pageNumber = 1, int pageSize = 10, string sortBy = "Name")
        {
            var query = _bankSystemDbContext.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(name)) query = query.Where(c => c.Name.Contains(name));
            if (!string.IsNullOrEmpty(surname)) query = query.Where(c => c.Surname.Contains(surname));
            if (!string.IsNullOrEmpty(phoneNumber)) query = query.Where(c => c.PhoneNumber.Contains(phoneNumber));
            if (!string.IsNullOrEmpty(pasNumber)) query = query.Where(c => c.Passport == pasNumber);

            if (sortBy == "Name")
            {
                query = query.OrderBy(c => c.Name);
            }
            else if (sortBy == "Date")
            {
                query = query.OrderBy(c => c.Date);
            }

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }
    }
}
