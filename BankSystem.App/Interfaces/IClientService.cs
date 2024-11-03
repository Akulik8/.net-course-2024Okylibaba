using BankSystem.App.DTOs;
using BankSystem.App.Services.Exceptions;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IClientService
    {
        public Task<Dictionary<Client, List<Account>>> GetAsync(Guid clientId);

        public Task<ClientDto> GetClientAsync(Guid id);

        public Task AddClientAsync(ClientDto client);

        public Task RemoveClientAsync(Guid id);

        public Task UpdateClientAsync(Guid id, ClientDto newClient);

        public Task<List<ClientDto>> GetAsync(int pageSize, int pageNumber, FindClientDto clientDto);

        public Task AddAccountToClientAsync(Client client, Account account);

        public Task EditAccountAsync(Account newAccount);

        public Task DebitingMoneyFromAccount(Client client, Account account, decimal cash);
    }
}