using AutoMapper;
using BankSystem.App.DTOs;
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
    public class ClientService: IClientService
    {
        private readonly IClientStorage _clientStorage;
        private readonly IMapper _mapper;

        public ClientService(IClientStorage clientStorage, IMapper mapper)
        {
            _clientStorage = clientStorage;
            _mapper = mapper;
        }

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async Task<Dictionary<Client, List<Account>>> GetAsync(Guid clientId)
        {
            return await _clientStorage.GetByIdAsync(clientId);
        }

        public async Task AddClientAsync(ClientDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);

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

        public async Task RemoveClientAsync(Guid id)
        {
            
            var existingClient = await _clientStorage.GetByIdAsync(id);
            if (!existingClient.Any())
                throw new NotFoundException("Клиент не найден.");

            await _clientStorage.DeleteAsync(id);
        }

        public async Task UpdateClientAsync(Guid id, ClientDto newClientDto)
        {
            var newClient = _mapper.Map<Client>(newClientDto);

            var existingClient = await _clientStorage.GetClientByIdAsync(id);

            if (existingClient is null)
                throw new NotFoundException("Клиент не найден.");

            if (newClient == null)
                throw new Exception("Нет сведений о новом клиенте.");

            await _clientStorage.UpdateAsync(id, newClient);
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

        public async Task<ClientDto> GetClientAsync(Guid Clientid)
        {
            var client = await _clientStorage.GetClientByIdAsync(Clientid);


            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> FindClientAsync(string? name, string? surname, string? phoneNumber, string? pasNumber, DateOnly? date)
        {
            int pageNumber = 1;
            int pageSize = 10;
            string sortBy = "Name";

            var clients = await _clientStorage.GetClientsByParametersAsync(name, surname, phoneNumber, pasNumber, date, pageNumber, pageSize, sortBy);

            var clientsDto = _mapper.Map<ClientDto>(clients.FirstOrDefault());
            return clientsDto;
        }
    }
}