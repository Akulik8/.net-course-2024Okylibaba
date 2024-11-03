using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IClientStorage : IStorage<Client, Dictionary<Client, List<Account>>>
    {
        public Task<Client> GetClientByIdAsync(Guid id);

        public Task AddAccountAsync(Guid id, Account account);
        
        public Task UpdateAccountAsync(Account newAccount);
        
        public Task DeleteAccountAsync(Guid id);

        public Task<List<Client>> GetClientsByParametersAsync(string? name = null, string? surname = null,
                                                                string? phoneNumber = null, string? pasNumber = null,
                                                                DateOnly? date = null, int pageNumber = 1,
                                                                int pageSize = 10, string sortBy = "Name");
    }
}
