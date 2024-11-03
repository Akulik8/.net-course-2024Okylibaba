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

        public Task<ClientDto> FindClientAsync(string? name, string? surname, string? phoneNumber, string? pasNumber, DateOnly? date);

        public Task<List<Client>> GetAsync(int pageSize, int pageNumber, Expression<Func<Client, bool>>? filter);
    }
}
