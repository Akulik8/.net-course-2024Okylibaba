using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class ClientStorage : IClientStorage
    {
        private readonly Dictionary<Client, List<Account>> _clients;

        public ClientStorage()
        {
            _clients = new Dictionary<Client, List<Account>>();
        }

        public void Add(Client client)
        {
            List<Account> accounts = new List<Account>();
            accounts.Add(new Account
            {
                Currency = new Currency
                {
                    Name = "Доллар США",
                    Code = "USD",
                    ExchangeRate = 1.0m
                },
                Amount = 0
            });

            _clients.Add(client, accounts);
        }

        public void Delete(Client client)
        {
            _clients.Remove(client);
        }

        public void Update(Client oldClient, Client newClient) 
        {
            oldClient.Name = newClient.Name;
            oldClient.Surname = newClient.Surname;
            oldClient.PhoneNumber = newClient.PhoneNumber;
            oldClient.Passport = newClient.Passport;
            oldClient.Address = newClient.Address;
            oldClient.Date = newClient.Date;    
            oldClient.AccountNumber = newClient.AccountNumber;
            oldClient.Balance = newClient.Balance;
        }

        public Dictionary<Client, List<Account>> Get(Func<Client, bool>? filter)
        {
            if (filter == null)
                return _clients;

            return _clients
                .Where(kvp => filter(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public void AddAccount(Client client, Account account)
        {
            _clients[client].Add(account);
        }

        public void UpdateAccount(Client client, Account oldAccount, Account newAccount) 
        {
            Account updatedAccount = _clients[client].FirstOrDefault(a => a.Currency.Equals(oldAccount.Currency));

            if (updatedAccount != null)
            {
                updatedAccount.Amount = newAccount.Amount;
                updatedAccount.Currency = newAccount.Currency;
            }
            else
            {
                throw new Exception("Счет не найден.");
            }
        }

        public void DeleteAccount(Client client, Account account) 
        {
            _clients[client].Remove(account);
        }
    }
}
