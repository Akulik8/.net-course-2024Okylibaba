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
        public void AddAccount(Guid id, Account account);
        
        public void UpdateAccount(Account newAccount);
        
        public void DeleteAccount(Guid id);
    }
}
