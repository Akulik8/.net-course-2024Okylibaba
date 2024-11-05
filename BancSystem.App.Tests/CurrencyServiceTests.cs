using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using BankSystem.Domain.Models;
using BankSystem.App.DTOs;
using Xunit.Abstractions;

namespace BankSystem.App.Tests
{
    public class CurrencyServiceTests
    {
        private readonly ITestOutputHelper _testOutput;
        CurrencyService _currencyService;

        public CurrencyServiceTests(ITestOutputHelper _testOutput)
        {
            this._testOutput = _testOutput;

            _currencyService = new CurrencyService(Settings.Default.apiKey, Settings.Default.baseUrl);
        }

        [Fact]
        public async Task GetCurrencyInfo()
        {
            CurrencyData currencyData = new CurrencyData()
            {
                To = "USD",
                From = "EUR",
                Amount = 100
            };

            CurrencyResponse currencyResponse = await _currencyService.GetCurrency(currencyData);

            _testOutput.WriteLine(currencyResponse.Amount.ToString(), OutputLevel.Information);
        }

    }
}
