using BankSystem.App.DTOs;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeAsync([FromQuery] Guid id)
        {
            return Ok(await _employeeService.GetEmployeeAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromQuery] EmployeeDto employee)
        {
            if (employee == null)
            {
                return BadRequest("employee cannot be null.");
            }

            await _employeeService.AddEmployeeAsync(employee);
            return Created();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromQuery] EmployeeDto EmployeeDto)
        {
            await _employeeService.UpdateEmployeeAsync(id, EmployeeDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee([FromQuery] Guid guid)
        {
            await _employeeService.RemoveEmployeeAsync(guid);
            return NoContent();
        }

        [HttpGet("FindEmployee")]
        public async Task<IActionResult> FindEmployee(string? name, string? surname, string? phoneNumber, string? pasNumber)
        {
            EmployeeDto response = await _employeeService.FindEmployeeAsync(name, surname, phoneNumber, pasNumber);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

    }
}
