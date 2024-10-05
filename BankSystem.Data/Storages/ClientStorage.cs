using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class ClientStorage
    {
        private readonly Dictionary<Client, List<Account>> _clients;

        public ClientStorage()
        {
            _clients = new Dictionary<Client, List<Account>>();
        }

        public void AddClient(Client client, List<Account> accounts)
        {
            _clients.Add(client, accounts);
        }

        public void RemoveClient(Client client)
        {
            _clients.Remove(client);
        }

        public void AddAccount(Client client, Account account)
        {
            _clients[client].Add(account);
        }

        public void UpdateAccount(Client client, Account oldAccount, Account newAccount) 
        {
            Account updatedAccount = _clients[client].FirstOrDefault(a => a.Currency.Equals(oldAccount.Currency));
            updatedAccount.Amount = newAccount.Amount;
            updatedAccount.Currency = newAccount.Currency;
        }

        public void RemoveAccount(Client client, Account account) 
        {
            _clients[client].Remove(account);
        }

        public Client GetYoungestClient()
        {
            return _clients.Keys.OrderBy(c => c.Date).LastOrDefault();
        }

        public Client GetOldestClient()
        {
            return _clients.Keys.OrderBy(c => c.Date).FirstOrDefault();
        }

        public double GetAverageAge()
        {
            if (_clients.Count == 0)
                return 0;

            DateTime today = DateTime.Today;
            return _clients.Keys.Average(c => (today.Year - c.Date.Year) - (today.DayOfYear < c.Date.DayOfYear ? 1 : 0));
        }

        public Dictionary<Client, List<Account>> GetAllClients()
        {
            return new Dictionary<Client, List<Account>>(_clients);
        }
    }
}
