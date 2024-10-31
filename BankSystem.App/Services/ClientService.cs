using BankSystem.App.Interfaces;
using BankSystem.App.Services.Exceptions;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Services
{
    public class ClientService
    {
        private readonly IClientStorage _clientStorage;

        public ClientService(IClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async Task<Dictionary<Client, List<Account>>> GetAsync(Client client)
        {
            return await _clientStorage.GetByIdAsync(client.Id);
        }

        public async Task AddClientAsync(Client client)
        {
            var existingClient = await _clientStorage.GetByIdAsync(client.Id);
            if (existingClient.Any())
                throw new PersonAlreadyExistsException("Этот клиент уже есть.");

            DateTime today = DateTime.Today;
            int age = (today.Year - client.Date.Year) - (today.DayOfYear < client.Date.DayOfYear ? 1 : 0);
            if (age < 18)
                throw new PersonTooYoungException("Клиент не должен быть моложе 18 лет.");

            if (string.IsNullOrEmpty(client.Passport))
                throw new NoPassportException("У клиента нет паспортных данных.");

            await _clientStorage.AddAsync(client);
        }

        public async Task RemoveClientAsync(Client client)
        {
            var existingClient = await _clientStorage.GetByIdAsync(client.Id);
            if (!existingClient.Any())
                throw new NotFoundException("Клиент не найден.");

            await _clientStorage.DeleteAsync(client.Id);
        }

        public async Task UpdateClientAsync(Client newClient)
        {
            var existingClient = await _clientStorage.GetByIdAsync(newClient.Id);
            if (!existingClient.Any())
                throw new NotFoundException("Клиент не найден.");

            if (newClient == null)
                throw new Exception("Нет сведений о новом клиенте.");

            await _clientStorage.UpdateAsync(newClient.Id, newClient);
        }

        public async Task AddAccountToClientAsync(Client client, Account account)
        {
            var existingClient = await _clientStorage.GetByIdAsync(client.Id);
            if (!existingClient.Any())
                throw new NotFoundException("Клиент не найден.");

            if (account == null)
                throw new Exception("Лицевой счет не может быть нулевым.");

            await _clientStorage.AddAccountAsync(client.Id, account);
        }

        public async Task EditAccountAsync(Account newAccount)
        {
            if (newAccount == null)
                throw new Exception("Нет сведений о новом лицевом счете.");

            await _clientStorage.UpdateAccountAsync(newAccount);
        }

        public async Task DebitingMoneyFromAccount(Client client, Account account, decimal cash)
        {
            await _semaphore.WaitAsync();
            try
            {
                var clientDictionary = await _clientStorage.GetByIdAsync(client.Id);

                if (clientDictionary.TryGetValue(client, out var accounts))
                {
                    foreach (var item in accounts)
                    {
                        if (item.Id == account.Id)
                        {
                            if (item.Amount < cash)
                                throw new Exception("Недостаточно сердств на счёте.");
                            item.Amount -= cash;
                            await _clientStorage.UpdateAccountAsync(item);
                            return;
                        }
                    }
                }
                else
                    throw new NotFoundException("Клиент не найден");
            }
            finally 
            {
                _semaphore.Release();
            }
        }

        public async Task DeleteAccountAsync(Account account)
        {
            await _clientStorage.DeleteAccountAsync(account.Id);
        }

        public async Task<List<Client>> GetAsync(int pageSize, int pageNumber, Expression<Func<Client, bool>>? filter)
        {
            return await _clientStorage.GetAsync(pageSize, pageNumber, filter);
        }
    }
}