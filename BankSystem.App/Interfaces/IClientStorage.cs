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
        public Task AddAccountAsync(Guid id, Account account);
        
        public Task UpdateAccountAsync(Account newAccount);
        
        public Task DeleteAccountAsync(Guid id);
    }
}
