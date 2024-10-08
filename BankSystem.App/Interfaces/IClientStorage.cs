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
        public void AddAccount(Client client, Account account);
        
        public void UpdateAccount(Client client, Account oldAccount, Account newAccount);
        
        public void DeleteAccount(Client client, Account account);
    }
}
