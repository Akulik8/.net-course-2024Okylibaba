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
    public class EmployeeStorage : IStorage<Employee, List<Employee>>
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
                .FirstOrDefaultAsync(e => e.Id == newEmployee.Id);

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
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }
    }
}
