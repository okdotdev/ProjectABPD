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
            throw new NotImplementedException();
        }

        private async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            throw new NotImplementedException();
        }
    }

    public class ExchangeRateResponse
    {
        public Dictionary<string, decimal> Rates { get; set; }
    }
}