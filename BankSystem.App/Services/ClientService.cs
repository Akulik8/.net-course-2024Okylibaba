using BankSystem.App.Services.Exceptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Services
{
    public class ClientService
    {
        private readonly ClientStorage _clientStorage;

        public ClientService(ClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }

        public void AddClient(Client client)
        {
            if (_clientStorage.GetAllClients().ContainsKey(client))
            {
                throw new PersonAlreadyExistsException("Этот клиент уже есть.");
            }

            DateTime today = DateTime.Today;
            int age = (today.Year - client.Date.Year) - (today.DayOfYear < client.Date.DayOfYear ? 1 : 0);
            if (age < 18)
            {
                throw new PersonTooYoungException("Клиент не должен быть моложе 18 лет.");
            }

            if (string.IsNullOrEmpty(client.Passport))
            {
                throw new NoPassportException("У клиента нет паспортных данных.");
            }

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

            _clientStorage.AddClient(client, accounts);
        }

        public void AddAccountToClient(string passport, Account account)
        {
            var client = _clientStorage.GetAllClients().FirstOrDefault(c => c.Key.Passport == passport);

            if (client.Key == null)
                throw new NotFoundException("Клиент с таким паспортом не найден");

            if (client.Value.Any(a => a.Currency.Equals(account.Currency)))
                throw new Exception("У клиента уже есть счёт в этой валюте");

            _clientStorage.AddAccount(client.Key, account);
        }


        public void EditAccount(string passport, Account oldAccount, Account newAccount)
        {
            var client = _clientStorage.GetAllClients().FirstOrDefault(c => c.Key.Passport == passport);

            if (client.Key == null)
                throw new NotFoundException("Клиент с таким паспортом не найден");

            var account = client.Value.FirstOrDefault(a => a.Currency.Equals(oldAccount.Currency));

            if (account != null)
            {
                _clientStorage.UpdateAccount(client.Key, oldAccount, newAccount);
            }
            else
            {
                throw new Exception("Счет не найден.");
            }
        }

        public List<Client> GetClients(string name, string surname, string phoneNumber, string passport, DateOnly startDate, DateOnly endDate)
        {
            return _clientStorage.GetAllClients().Keys
                .Where(c =>
                    (string.IsNullOrEmpty(name) || c.Name == name) &&
                    (string.IsNullOrEmpty(surname) || c.Surname == surname) &&
                    (string.IsNullOrEmpty(phoneNumber) || c.PhoneNumber == phoneNumber) &&
                    (string.IsNullOrEmpty(passport) || c.Passport == passport) &&
                    c.Date >= startDate && c.Date <= endDate)
                .ToList();
        }
    }
}