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

        public List<Employee> GetAllEmployees()
        {
            return new List<Employee>(_employees);
        }
    }
}
