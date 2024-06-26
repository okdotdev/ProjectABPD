using abcAPI.Models.DTOs;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using abcAPI.Models.TableModels;

namespace abcAPI.Services
{
    public class RevenueService : IRevenueService
    {
        private readonly IContractService _contractService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IHttpClientFactory _httpClientFactory;

        public RevenueService(IContractService contractService, ISubscriptionService subscriptionService,
            IHttpClientFactory httpClientFactory)
        {
            _contractService = contractService;
            _subscriptionService = subscriptionService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<RevenueResponseDto> CalculateRevenueAsync(RevenueRequestDto requestDto)
        {
            var contracts = await _contractService.GetContractsAsync();
            var subscriptions = new List<Subscription>(); // await _subscriptionService.GetAllSubscriptionsAsync();

            if (!string.IsNullOrEmpty(requestDto.ProductName))
            {
                contracts = contracts.Where(c => c.SoftwareName == requestDto.ProductName).ToList();
                subscriptions = subscriptions.Where(s => s.Software.Name == requestDto.ProductName).ToList();
            }

            decimal currentRevenue = contracts.Where(c => c.IsPaid).Sum(c => c.Price) + subscriptions.Sum(s => s.Price);
            decimal projectedRevenue = contracts.Sum(c => c.Price) + subscriptions.Sum(s => s.Price);

            if (!string.IsNullOrEmpty(requestDto.Currency) && requestDto.Currency.ToUpper() != "PLN")
            {
                var exchangeRate = await GetExchangeRateAsync("PLN", requestDto.Currency);
                currentRevenue *= exchangeRate;
                projectedRevenue *= exchangeRate;
            }

            return new RevenueResponseDto
            {
                CurrentRevenue = currentRevenue,
                ProjectedRevenue = projectedRevenue,
                Currency = string.IsNullOrEmpty(requestDto.Currency) ? "PLN" : requestDto.Currency.ToUpper()
            };
        }

        private async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            var client = _httpClientFactory.CreateClient();
            var response =
                await client.GetStringAsync(
                    $"https://api.exchangeratesapi.io/latest?base={fromCurrency}&symbols={toCurrency}");
            var exchangeData = JsonSerializer.Deserialize<ExchangeRateResponse>(response);
            return exchangeData.Rates[toCurrency];
        }
    }

    public class ExchangeRateResponse
    {
        public Dictionary<string, decimal> Rates { get; set; }
    }
}