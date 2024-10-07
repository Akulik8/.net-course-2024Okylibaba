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
            if (_clientStorage.FindClientByPassport(client.Passport) != null)
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
            var client = _clientStorage.FindClientByPassport(passport);

            if (client == null)
                throw new NotFoundException("Клиент с таким паспортом не найден");

            var accounts = _clientStorage.GetClientAccounts(client);
            if (accounts.Any(a => a.Currency.Equals(account.Currency)))
                throw new Exception("У клиента уже есть счёт в этой валюте");

            _clientStorage.AddAccount(client, account);
        }


        public void EditAccount(string passport, Account oldAccount, Account newAccount)
        {
            var client = _clientStorage.FindClientByPassport(passport);

            if (client == null)
                throw new NotFoundException("Клиент с таким паспортом не найден");

            _clientStorage.UpdateAccount(client, oldAccount, newAccount);
        }

        public List<Client> GetClientsByFilter(string name, string surname, string phoneNumber, string passport, DateOnly startDate, DateOnly endDate)
        {
            return _clientStorage.GetClientsByFilter(name, surname, phoneNumber, passport, startDate, endDate);
        }
    }
}