using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs
{
    public class EmployeeDto
    {
        public string? FullName { get; set; }
        public DateOnly Date { get; set; }
        public int Salary { get; set; }
        public string? Position { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PasNumber { get; set; }
        public string? Address { get; set; }
    }
}
