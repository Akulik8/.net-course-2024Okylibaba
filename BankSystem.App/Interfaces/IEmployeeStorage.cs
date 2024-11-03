using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IEmployeeStorage: IStorage<Employee, List<Employee>>
    {
        public Task<Employee> GetEmployeeByIdAsync(Guid id);
    }
}
