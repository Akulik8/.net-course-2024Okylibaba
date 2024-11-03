using AutoMapper;
using BankSystem.App.DTOs;
using BankSystem.App.Interfaces;
using BankSystem.App.Services.Exceptions;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Services
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IEmployeeStorage _employeeStorage;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeStorage employeeStorage, IMapper mapper)
        {
            _employeeStorage = employeeStorage;
            _mapper = mapper;
        }

        public async Task<List<Employee>> GetAsync(Guid id)
        {
            return await _employeeStorage.GetByIdAsync(id);
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid id)
        {
            var employee = await _employeeStorage.GetEmployeeByIdAsync(id);

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task AddEmployeeAsync(EmployeeDto employeeDto)
        {
            var employee = _mapper.Map<Employee>(employeeDto);

            DateTime today = DateTime.Today;
            int age = (today.Year - employee.Date.Year) - (today.DayOfYear < employee.Date.DayOfYear ? 1 : 0);
            if (age < 18)
                throw new PersonTooYoungException("Сотрудник не должен быть моложе 18 лет.");
            if (string.IsNullOrEmpty(employee.Passport))
                throw new NoPassportException("У сотрудника нет паспортных данных.");

            await _employeeStorage.AddAsync(employee);
        }

        public async Task RemoveEmployeeAsync(Guid id)
        {
            var existingEmployee = await _employeeStorage.GetByIdAsync(id);
            if (!existingEmployee.Any())
                throw new NotFoundException("Сотрудник не найден.");

            await _employeeStorage.DeleteAsync(id);
        }

        public async Task UpdateEmployeeAsync(Guid id, EmployeeDto newEmployeeDto)
        {


            var existingEmployee = await _employeeStorage.GetByIdAsync(id);
            if (!existingEmployee.Any())
                throw new NotFoundException("Сотрудник не найден.");

            if (newEmployeeDto == null)
                throw new Exception("Нет сведений о новом сотруднике.");

            var newEmployee = _mapper.Map<Employee>(newEmployeeDto);

            await _employeeStorage.UpdateAsync(id, newEmployee);
        }

        public async Task<List<EmployeeDto>> GetEmployeesByFilterAsync(Expression<Func<Employee, bool>>? filter, int pageSize = 1, int pageNumber = 10)
        {
            var employees = await _employeeStorage.GetAsync(pageSize, pageNumber, filter);

            return employees.Select(_mapper.Map<EmployeeDto>).ToList();
        }

        public async Task<EmployeeDto> FindEmployeeAsync(string? name, string? surname, string? phoneNumber, string? pasNumber)
        {
            var employees = await _employeeStorage.GetEmployeesByParametersAsync(name, surname, phoneNumber, pasNumber);

            var employeeDto = _mapper.Map<EmployeeDto>(employees.FirstOrDefault());
            return employeeDto;
        }
    }
}
