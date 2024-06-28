using System.Text.Json;
using abcAPI.Models.DTOs;


namespace abcAPI.Services;

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

        if (requestDto == null)
        {
            throw new ArgumentNullException(nameof(requestDto), "Request DTO cannot be null.");
        }

        // jeśli wybrano produkt to tylko kontrakty i subskrypcje związane z tym produktem
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

        // tylko zapłacone i podpisane kontrakty (w tym subskrypcje)
        contracts = contracts.Where(c => c.IsSigned && c.IsPaid).ToList();


        decimal sum = 0;

        foreach (GetContractDto contract in contracts)
        {
            List<PaymentDto> payments = await _contractService.GetPaymentsForContract(contract);
            sum += payments.Sum(p => p.Amount);
            Console.WriteLine($"Testowy świr Contract {contract.Id} revenue: {sum}");
        }

        // Convert the sum to the target currency if necessary
        if (!string.IsNullOrEmpty(requestDto.TargetCurrency) && requestDto.TargetCurrency != "PLN")
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string url = $"https://api.exchangerate-api.com/v4/latest/PLN";
            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to retrieve exchange rate: {response.ReasonPhrase}");
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            var exchangeRate = JsonSerializer.Deserialize<ExchangeRate>(responseBody);

            if (exchangeRate == null || !exchangeRate.Rates.ContainsKey(requestDto.TargetCurrency))
            {
                throw new InvalidOperationException("Exchange rate data is invalid.");
            }

            sum *= exchangeRate.Rates[requestDto.TargetCurrency];
        }

        return sum;
    }
}

public class ExchangeRate
{
    public Dictionary<string, decimal> Rates { get; set; }
}