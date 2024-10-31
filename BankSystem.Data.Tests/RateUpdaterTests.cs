using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Tests
{
    public class RateUpdaterTests
    {
        [Fact]
        public async Task RateUpdaterTest()
        {
            var cancellationtTokenSource = new CancellationTokenSource();
            var token = cancellationtTokenSource.Token;
            IClientStorage storage = new ClientStorage(new BankSystemDbContext());
            var clientService = new ClientService(storage);
            var rateUpdater = new RateUpdater(storage);
            var testDataGenerator = new TestDataGenerator();

            var clients = testDataGenerator.GenerateClients(100);
            var clientsDictionary = testDataGenerator.GenerateClientAccounts(clients);
            
            foreach (var client in clientsDictionary)
            {
                await clientService.AddClientAsync(client.Key);

                foreach (var account in client.Value)
                {
                    await clientService.AddAccountToClientAsync(client.Key, account);
                }
            }

            var rateUpdaterTask = Task.Run(async () => await rateUpdater.UpdateAmountAccountAsync(token));

            await Task.Delay(10000);
            cancellationtTokenSource.Cancel();
            
            var firstClient = clientsDictionary.Keys.First();
            var updateAccouts = (await clientService.GetAsync(firstClient))[firstClient];
           
            foreach (var account in updateAccouts) 
            {
                Assert.True(account.Amount > 50);
            }

        }
    }
}
