using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class EmployeeStorage
    {
        private readonly List<Employee> _employees;

        public EmployeeStorage()
        {
            _employees = new List<Employee>();
        }

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
        }

        public void RemoveEmployee(Employee employee)
        {
            _employees.Remove(employee);
        }

        public void UpdateEmployee(Employee employee, Employee newEmployee) 
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

        public Employee GetYoungestEmployee()
        {
            return _employees.OrderBy(e => e.Date).LastOrDefault();
        }

        public Employee GetOldestEmployee()
        {
            return _employees.OrderBy(e => e.Date).FirstOrDefault();
        }

        public double GetAverageAge()
        {
            if (_employees.Count == 0)
                return 0;

            DateTime today = DateTime.Today;
            return _employees.Average(e => (today.Year - e.Date.Year) - (today.DayOfYear < e.Date.DayOfYear ? 1 : 0));
        }

        public Employee FindEmployeeByPassport(string passport)
        {
            return _employees.FirstOrDefault(c => c.Passport == passport);
        }

        public List<Employee> GetAllEmployees()
        {
            return new List<Employee>(_employees);
        }

        public List<Employee> GetEmployeesByFilter(string name, string surname, string phoneNumber, string passport, DateOnly startDate, DateOnly endDate)
        {
            return _employees
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
