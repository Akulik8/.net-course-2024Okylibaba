using BankSystem.App.DTOs;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> GetEmployeeAsync(Guid userId);
        Task AddEmployeeAsync(EmployeeDto employeeDto);
        Task UpdateEmployeeAsync(Guid id, EmployeeDto employeeDto);
        Task RemoveEmployeeAsync(Guid id);
        Task<List<EmployeeDto>> GetEmployeesByFilterAsync(Expression<Func<Employee, bool>>? filter, int pageSize  = 1, int pageNumber = 10);
        Task<EmployeeDto> FindEmployeeAsync(string? name, string? surname, string? phoneNumber, string? pasNumber);
    }
}
