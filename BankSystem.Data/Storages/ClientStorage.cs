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

        public async Task AddAsync(Client client)
        {
            if (client.Id == Guid.Empty)
            {
                client.Id = Guid.NewGuid();
            }

            await _bankSystemDbContext.Clients.AddAsync(client);
            await _bankSystemDbContext.SaveChangesAsync();

            var defaultAccount = new Account
            {
                ClientId = client.Id,
                Id = Guid.NewGuid(),
                Amount = 0,
                CurrencyName = "Доллар США"
            };

            await _bankSystemDbContext.Accounts.AddAsync(defaultAccount);
            await _bankSystemDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var client = await _bankSystemDbContext.Clients
                            .FirstOrDefaultAsync(c => c.Id == id);

            if (client != null)
            {
                _bankSystemDbContext.Clients.Remove(client);
                await _bankSystemDbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Guid id, Client newClient)
        {
            var client = await _bankSystemDbContext.Clients
                   .FirstOrDefaultAsync(c => c.Id == id);
            if (client != null)
            {
                client.Name = newClient.Name;
                client.Surname = newClient.Surname;
                client.PhoneNumber = newClient.PhoneNumber;
                client.Passport = newClient.Passport;
                client.Address = newClient.Address;
                client.Date = newClient.Date;

                await _bankSystemDbContext.SaveChangesAsync();
            }
        }

        public async Task<Dictionary<Client, List<Account>>> GetByIdAsync(Guid id)
        {
            var clientWithAccounts = await _bankSystemDbContext.Clients
                .Include(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (clientWithAccounts != null)
            {
                return new Dictionary<Client, List<Account>>
                {
                    { clientWithAccounts, clientWithAccounts.Accounts.ToList() }
                };
            }

            return new Dictionary<Client, List<Account>>();
        }

        public async Task<List<Client>> GetAsync(int pageSize, int pageNumber, Expression<Func<Client, bool>>? filter)
        {
            var query = _bankSystemDbContext.Clients.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query
                .OrderBy(x => x.Surname)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }


        public async Task AddAccountAsync(Guid id, Account account)
        {
            account.ClientId = id;
            await _bankSystemDbContext.Accounts.AddAsync(account);
            await _bankSystemDbContext.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Account newAccount)
        {
            var account = await _bankSystemDbContext.Accounts
                          .FirstOrDefaultAsync(a => a.Id == newAccount.Id);

            if (account != null)
            {
                account.CurrencyName = newAccount.CurrencyName;
                account.Amount = newAccount.Amount;

                await _bankSystemDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            var account = await _bankSystemDbContext.Accounts
                      .FirstOrDefaultAsync(a => a.Id == id);

            if (account != null)
            {
                _bankSystemDbContext.Accounts.Remove(account);
                await _bankSystemDbContext.SaveChangesAsync();
            }
        }

        public async Task<Client> GetClientByIdAsync(Guid id)
        {
            Client? client = await _bankSystemDbContext.Clients.FirstOrDefaultAsync(a => a.Id == id);

            if (client != null)
                return client;

            return new Client();
        }

        public async Task<List<Client>> GetClientsByParametersAsync(
            string? name = null, string? surname = null,
            string? phoneNumber = null, string? pasNumber = null,
            DateOnly? date = null, int pageNumber = 1, int pageSize = 10, string sortBy = "Name")
        {
            var query = _bankSystemDbContext.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(name)) query = query.Where(c => c.Name.Contains(name));
            if (!string.IsNullOrEmpty(surname)) query = query.Where(c => c.Surname.Contains(surname));
            if (!string.IsNullOrEmpty(phoneNumber)) query = query.Where(c => c.PhoneNumber.Contains(phoneNumber));
            if (!string.IsNullOrEmpty(pasNumber)) query = query.Where(c => c.Passport == pasNumber);
            if (date.HasValue) query = query.Where(c => c.Date == date.Value);

            if (sortBy == "Name")
            {
                query = query.OrderBy(c => c.Name);
            }
            else if (sortBy == "Date")
            {
                query = query.OrderBy(c => c.Date);
            }
           
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }
    }
}
