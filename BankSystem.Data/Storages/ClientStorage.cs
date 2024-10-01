using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class ClientStorage
    {
        private readonly List<Client> _clients;

        public ClientStorage()
        {
            _clients = new List<Client>();
        }

        public void AddClient(Client client)
        {
            _clients.Add(client);
        }

        public void RemoveClient(Client client)
        {
            _clients.Remove(client);
        }

        public Client GetYoungestClient()
        {
            return _clients.OrderBy(c => c.Date).LastOrDefault();
        }

        public Client GetOldestClient()
        {
            return _clients.OrderBy(c => c.Date).FirstOrDefault();
        }

        public double GetAverageAge()
        {
            if (_clients.Count == 0)
                return 0;

            DateTime today = DateTime.Today;
            return _clients.Average(c => (today.Year - c.Date.Year) - (today.DayOfYear < c.Date.DayOfYear ? 1 : 0));
        }

        public List<Client> GetAllClients()
        {
            return _clients;
        }
    }
}
