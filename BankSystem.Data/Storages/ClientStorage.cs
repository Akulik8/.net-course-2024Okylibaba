using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class ClientStorage : IClientStorage
    {
        private readonly BankSystemDbContext _bankSystemDbContext;

        public ClientStorage(BankSystemDbContext bankSystemDbContext)
        {
            _bankSystemDbContext = bankSystemDbContext;
        }

        public void Add(Client client)
        {
            if (client.Id == Guid.Empty)
            {
                client.Id = Guid.NewGuid();
            }

            _bankSystemDbContext.Clients.Add(client);
            _bankSystemDbContext.SaveChanges();

            var defaultAccount = new Account
            {
                ClientId = client.Id,
                Id = Guid.NewGuid(),
                Amount = 0,
                CurrencyName = "Доллар США"
            };

            _bankSystemDbContext.Accounts.Add(defaultAccount);
            _bankSystemDbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var client = _bankSystemDbContext.Clients
                        .FirstOrDefault(c => c.Id == id);

            if (client != null)
            {
                _bankSystemDbContext.Clients.Remove(client);
                _bankSystemDbContext.SaveChanges();
            }
        }

        public void Update(Guid id, Client newClient) 
        {
            var client = _bankSystemDbContext.Clients
               .FirstOrDefault(c => c.Id == newClient.Id);
            if (client != null)
            {
                client.Name = newClient.Name;
                client.Surname = newClient.Surname;
                client.PhoneNumber = newClient.PhoneNumber;
                client.Passport = newClient.Passport;
                client.Address = newClient.Address;
                client.Date = newClient.Date;

                _bankSystemDbContext.SaveChanges();
            }
        }

        public Dictionary<Client, List<Account>> GetById(Guid id)
        {
            var clientWithAccounts = _bankSystemDbContext.Clients
                .Include(c => c.Accounts)
                .FirstOrDefault(c => c.Id == id);

            if (clientWithAccounts != null)
            {
                return new Dictionary<Client, List<Account>>
                {
                    { clientWithAccounts, clientWithAccounts.Accounts.ToList() }
                };
            }

            return new Dictionary<Client, List<Account>>();
        }

        public List<Client> Get(int pageSize, int pageNumber, Func<Client, bool>? filter)
        {
            var query = _bankSystemDbContext.Clients.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter).AsQueryable();
            }

            query = query
                .OrderBy(x => x.Surname)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return query.ToList();
        }

        public void AddAccount(Guid Id, Account account)
        {
            account.ClientId = Id;
            _bankSystemDbContext.Accounts.Add(account);
            _bankSystemDbContext.SaveChanges();
        }

        public void UpdateAccount(Account newAccount) 
        {
            var account = _bankSystemDbContext.Accounts
                      .FirstOrDefault(a => a.Id == newAccount.Id);

            if (account != null)
            {
                account.CurrencyName = newAccount.CurrencyName;
                account.Amount = newAccount.Amount;

                _bankSystemDbContext.SaveChanges();
            }
        }

        public void DeleteAccount(Guid id) 
        {
            var account = _bankSystemDbContext.Accounts
                  .FirstOrDefault(a => a.Id == id);

            if (account != null)
            {
                _bankSystemDbContext.Accounts.Remove(account);
                _bankSystemDbContext.SaveChanges();
            }
        }
    }
}
