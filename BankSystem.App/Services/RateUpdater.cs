using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Services
{
    public class RateUpdater
    {
        private readonly IClientStorage _clientStorage;

        public RateUpdater(IClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }

        public async Task UpdateAmountAccountAsync(CancellationToken token)
        {
            var clients = await _clientStorage.GetAsync(int.MaxValue, 1, null);

            while (!token.IsCancellationRequested)
            {
                foreach (var client in clients)
                {
                    var clientWithAccount = await _clientStorage.GetByIdAsync(client.Id);
                    if (clientWithAccount.Values == null)
                        continue;

                    foreach (var accounts in clientWithAccount.Values)
                    {
                        foreach (var account in accounts)
                        {
                            account.Amount += 100;
                            await _clientStorage.UpdateAccountAsync(account);
                        }
                    }
                }
                await Task.Delay(5000);
            }
        }
    }
}