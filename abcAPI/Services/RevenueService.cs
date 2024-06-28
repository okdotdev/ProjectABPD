using System.Text.Json;
using System.Text.Json.Serialization;
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

    public async Task<decimal> CalculateProjectedRevenueAsync(RevenueRequestDto requestDto)
    {
        List<GetContractDto> contracts;
        List<GetSubscriptionDto> subscriptions;

        decimal sum = 0;

        if (requestDto == null)
        {
            throw new ArgumentNullException(nameof(requestDto), "Request DTO cannot be null.");
        }

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

        foreach (GetContractDto contract in contracts)
        {
            List<PaymentDto> payments = await _contractService.GetPaymentsForContract(contract);
            sum += payments.Sum(p => p.Amount);
            Console.WriteLine($" Contract {contract.Id} revenue: {sum}");
        }

        //project revenue from subscriptions

        foreach (GetSubscriptionDto subscription in subscriptions)
        {
            DateTime currentDate = DateTime.Now;
            decimal renewalPrice = subscription.RenewalPrice;
            bool isMonthly = subscription.IsMonthly;

            //jeśli isMontly to dodajemy renewal price za kazdy miesiąc od teraz do końca jeśli nie to każdy rok
            if (isMonthly)
            {
                while (currentDate < subscription.EndDate)
                {
                    sum += renewalPrice;
                    currentDate = currentDate.AddMonths(1);
                }
            }
            else
            {
                while (currentDate < subscription.EndDate)
                {
                    sum += renewalPrice;
                    currentDate = currentDate.AddYears(1);
                }
            }
        }


        return sum;
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
            Console.WriteLine($" Contract {contract.Id} revenue: {sum}");
        }

        //Niestety nie udało mi się zaimplementować przelicznika walut z jakiegoś powodu wyrzucało mi caly czas Nulla
        //przy Rates, więc zdecydowałem się na usunięcie tej funkcjonalności

        /*
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

            // Log the raw JSON response
            Console.WriteLine($"Exchange rate API response: {responseBody}");

            ExchangeRate? exchangeRate = JsonSerializer.Deserialize<ExchangeRate>(responseBody);

            if (exchangeRate == null || exchangeRate.Rates == null || !exchangeRate.Rates.TryGetValue(requestDto.TargetCurrency, value: out decimal rate))
            {
                throw new InvalidOperationException("Exchange rate data is invalid.");
            }

            sum *= rate;
        } */

        return sum;
    }
}

public class ExchangeRate
{
    [JsonPropertyName("rates")] public Dictionary<string, decimal> Rates { get; set; }
}