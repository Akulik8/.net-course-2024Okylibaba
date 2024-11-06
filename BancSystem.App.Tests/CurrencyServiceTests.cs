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
        private readonly CurrencyService _currencyService;

        public CurrencyServiceTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;

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

            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                var token = tokenSource.Token;

                try
               {
                    CurrencyResponse currencyResponse = await _currencyService.ConvertAsync(currencyData, token);

                    _testOutput.WriteLine(currencyResponse.Amount.ToString(), OutputLevel.Information);
                }
                catch (Exception ex)
                {
                    if (ex is TaskCanceledException)
                        _testOutput.WriteLine("Operation aborted");
                    else
                        _testOutput.WriteLine($"Failed to convert {ex.Message}");
                }
            }
                
        }

    }
}
