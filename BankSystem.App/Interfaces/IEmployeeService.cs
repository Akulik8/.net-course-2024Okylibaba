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
        Task<List<EmployeeDto>> GetEmployeesByFilterAsync(FindEmployeeDto findEmployeeDto, int pageSize = 100, int pageNumber = 1);
    }
}
