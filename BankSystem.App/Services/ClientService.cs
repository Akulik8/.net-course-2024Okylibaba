using BankSystem.App.Interfaces;
using BankSystem.App.Services.Exceptions;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddClient(Client client)
        {
            if (_clientStorage.Get(c => c.Passport == client.Passport).Any())
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
            if (!_clientStorage.Get(c => c.Passport == client.Passport).Any())
                throw new NotFoundException("Клиент не найден.");

            _clientStorage.Delete(client);
        }

        public void UpdateClient(Client oldClient, Client newClient)
        {
            if (!_clientStorage.Get(c => c.Passport == oldClient.Passport).Any())
                throw new NotFoundException("Клиент не найден.");
            if (newClient == null)
                throw new Exception("Нет сведений о новом клиенте.");

            _clientStorage.Update(oldClient, newClient);
        }

        public void AddAccountToClient(Client client, Account account)
        {
            var findClient= _clientStorage.Get(c => c.Passport == client.Passport);
            if (!findClient.Any())
                throw new NotFoundException("Клиент не найден.");
            if (account == null)
                throw new Exception("Лицевой счет не может быть нулевым.");
            if (findClient.Values.Any(a => a.Equals(account)))
                    throw new Exception("У клиента уже есть счёт в этой валюте.");

            _clientStorage.AddAccount(client, account);
        }

        public void EditAccount(Client client, Account oldAccount, Account newAccount)
        {
            var findClient = _clientStorage.Get(c => c.Passport == client.Passport);
            if (!findClient.Any())
                throw new NotFoundException("Клиент не найден.");
            if (!findClient.Values.Any(a => a.Contains(oldAccount)))
                throw new Exception("У клиента нет счёта в выбранной валюте.");
            if (newAccount == null)
                throw new Exception("Нет сведений о новом лицевом счете.");

            _clientStorage.UpdateAccount(client, oldAccount, newAccount);
        }

        public void DeleteAccount(Client client, Account account)
        {
            var findClient = _clientStorage.Get(c => c.Passport == client.Passport);
            if (!findClient.Any())
                throw new NotFoundException("Клиент не найден.");
            if (!findClient.Values.Any(a => a.Equals(account)))
                throw new Exception("У клиента нет счёта в выбранной валюте.");

            _clientStorage.DeleteAccount(client, account);
        }

        public Dictionary<Client, List<Account>> GetClientsByFilter(string? name, string? surname, string? phoneNumber, string? passport, DateOnly? startDate, DateOnly? endDate)
        {
            return _clientStorage.Get(c =>
              (string.IsNullOrEmpty(name) || c.Name == name) &&
              (string.IsNullOrEmpty(surname) || c.Surname == surname) &&
              (string.IsNullOrEmpty(phoneNumber) || c.PhoneNumber == phoneNumber) &&
              (string.IsNullOrEmpty(passport) || c.Passport == passport) &&
              (!startDate.HasValue || c.Date >= startDate.Value) &&
              (!endDate.HasValue || c.Date <= endDate.Value));
        }
    }
}