using Newtonsoft.Json;
using BankSystem.Domain.Models;
using BankSystem.App.DTOs;
using System.Runtime.CompilerServices;

namespace BankSystem.App.Services
{
    public class CurrencyService
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public CurrencyService(string apiKey, string baseUrl)
        {
            _apiKey = apiKey;
            _baseUrl = baseUrl;
        }

        public async Task<CurrencyResponse> GetCurrency(CurrencyData data)
        {
            UriBuilder uriBuilder = new UriBuilder(_baseUrl);

            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            query["api_key"] = _apiKey;
            query["from"] = data.From;
            query["to"] = data.To;
            query["amount"] = data.Amount.ToString();

            uriBuilder.Query = query.ToString();

            string finalUrl = uriBuilder.ToString();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(finalUrl);
                string message = await responseMessage.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<CurrencyResponse>(message);
            }
        }
    }
}