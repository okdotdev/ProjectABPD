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

        public Task<decimal> CalculateProjectedRevenueAsync(RevenueRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public async Task<decimal> CalculateRealRevenueAsync(RevenueRequestDto requestDto)
        {
            List<GetContractDto> contracts;
            List<GetSubscriptionDto> subscriptions;

            //jesli został wybrany produkt to pobieramy tylko umowy i subskrypcje dla tego produktu
            //jeśli nie to dla całej firmy
            if (requestDto.ProductId != 0)
            {
                contracts = (await _contractService.GetContractsAsync())
                    .Where(c => c.SoftwareId == requestDto.ProductId).ToList();
                subscriptions = (await _subscriptionService.GetSubscriptionsListAsync())
                    .Where(s => s.SoftwareId == requestDto.ProductId).ToList();
            }
            else
            {
                contracts = await _contractService.GetContractsAsync();
                subscriptions = await _subscriptionService.GetSubscriptionsListAsync();
            }

            //wybieramy tylko opłacone i podpisane umowy

            contracts = contracts.Where(c => c.IsSigned && c.IsPaid).ToList();

            //wybieramy płatności dotyczące tych kontraktów i sumujemy je


            decimal sum = 0;

            foreach (GetContractDto contract in contracts)
            {
                List<PaymentDto> payments = await _contractService.GetPaymentsForContract(contract);
                sum += payments.Sum(p => p.Amount);
            }

            //przeliczamy sumę na wybraną walute

            if (requestDto.TargetCurrency == "PLN") return sum;

            HttpClient client = _httpClientFactory.CreateClient();
            string url = $"https://api.exchangeratesapi.io/latest?base=PLN&symbols={requestDto.TargetCurrency}";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            ExchangeRate exchangeRate = JsonSerializer.Deserialize<ExchangeRate>(responseBody);
            sum *= exchangeRate.Rates[requestDto.TargetCurrency];


            return sum;
        }
    }
}

public class ExchangeRate
{
    public Dictionary<string, decimal> Rates { get; set; }
}