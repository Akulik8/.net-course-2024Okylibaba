using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class EmployeeStorage : IStorage<Employee, List<Employee>>
    {
        private readonly List<Employee> _employees;

        public EmployeeStorage()
        {
            _employees = new List<Employee>();
        }

        public void Add(Employee employee)
        {
            _employees.Add(employee);
        }

        public void Delete(Employee employee)
        {
            _employees.Remove(employee);
        }

        public void Update(Employee employee, Employee newEmployee) 
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
        }

        public List<Employee> Get(Func<Employee, bool>? filter)
        {
            if (filter == null)
                return _employees;

            return _employees.Where(filter).ToList();
        }
    }
}
