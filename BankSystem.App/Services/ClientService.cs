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

        public Dictionary<Client, List<Account>> Get(Client client)
        {
            return _clientStorage.GetById(client.Id);
        }

        public void AddClient(Client client)
        {
            if (_clientStorage.GetById(client.Id).Any())
                throw new PersonAlreadyExistsException("Этот клиент уже есть.");

            DateTime today = DateTime.Today;
            int age = (today.Year - client.Date.Year) - (today.DayOfYear < client.Date.DayOfYear ? 1 : 0);
            if (age < 18)
                throw new PersonTooYoungException("Клиент не должен быть моложе 18 лет.");

            if (string.IsNullOrEmpty(client.Passport))
                throw new NoPassportException("У клиента нет паспортных данных.");

            _clientStorage.Add(client);
        }

        public void RemoveClient(Client client)
        {
            if (!_clientStorage.GetById(client.Id).Any())
                throw new NotFoundException("Клиент не найден.");

            _clientStorage.Delete(client.Id);
        }

        public void UpdateClient(Client newClient)
        {
            if (!_clientStorage.GetById(newClient.Id).Any())
                throw new NotFoundException("Клиент не найден.");
            if (newClient == null)
                throw new Exception("Нет сведений о новом клиенте.");

            _clientStorage.Update(newClient.Id, newClient);
        }

        public void AddAccountToClient(Client client, Account account)
        {
            if (!_clientStorage.GetById(client.Id).Any())
                throw new NotFoundException("Клиент не найден.");
            if (account == null)
                throw new Exception("Лицевой счет не может быть нулевым.");
            _clientStorage.AddAccount(client.Id, account);
        }

        public void EditAccount(Account newAccount)
        {
            if (newAccount == null)
                throw new Exception("Нет сведений о новом лицевом счете.");

            _clientStorage.UpdateAccount(newAccount);
        }

        public void DeleteAccount(Account account)
        {
            _clientStorage.DeleteAccount(account.Id);
        }

        public List<Client> Get(int pageSize, int pageNumber, Func<Client, bool>? filters)
        {
            return _clientStorage.Get(pageSize, pageNumber, filters);
        }
    }
}