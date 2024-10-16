using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
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

        public void Add(Employee employee)
        {
            _bankSystemDbContext.Employees.Add(employee);
            _bankSystemDbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var employee = _bankSystemDbContext.Employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                _bankSystemDbContext.Employees.Remove(employee);
                _bankSystemDbContext.SaveChanges();
            }
        }

        public void Update(Guid id, Employee newEmployee) 
        {
            var employee = _bankSystemDbContext.Employees
                .FirstOrDefault(e => e.Id == newEmployee.Id);

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

                _bankSystemDbContext.SaveChanges();
            }
        }

        public List<Employee> GetById(Guid id)
        {
            var employee = _bankSystemDbContext.Employees
                .Where(e => e.Id == id)
                .ToList();

            return employee;
        }

        public List<Employee> Get(int pageSize, int pageNumber, Func<Employee, bool>? filter)
        {
            var query = _bankSystemDbContext.Employees.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter).AsQueryable();
            }

            query = query
                .OrderBy(x => x.Surname)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return query.ToList();
        }
    }
}
