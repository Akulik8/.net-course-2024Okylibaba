using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs
{
    public class ClientDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly Date { get; set; }
        public string PasNumber { get; set; }
        public string Address { get; set; }
        public int? Bonus { get; set; } = 0;
    }
}
