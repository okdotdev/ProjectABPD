using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;
using abcAPI.Repositories;

namespace abcAPI.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IContractService _contractService;
    private readonly IDiscountService _discountService;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository, IContractService contractService,
        IDiscountService discountService)
    {
        _subscriptionRepository = subscriptionRepository;
        _contractService = contractService;
        _discountService = discountService;
    }


    public async Task SubscribeAsync(SubscribeDto subscribeDto)
    {
        if (subscribeDto.StartDate > subscribeDto.EndDate)
        {
            throw new ArgumentException("End date should be greater than start date");
        }

        switch (subscribeDto.EndDate.Subtract(subscribeDto.StartDate).TotalDays)
        {
            case < 30:
                throw new ArgumentException("Subscription time should be at least 1 month");
            case > 730:
                throw new ArgumentException("Subscription time should be at most 2 years");
            default:
                await _subscriptionRepository.Subscribe(subscribeDto);
                break;
        }

        //tworzymy kontrakt

        int additionalSupportYears = subscribeDto.EndDate.Year - subscribeDto.StartDate.Year;

        CreateContractDto createContractDto = new()
        {
            ClientId = subscribeDto.ClientId,
            StartDate = subscribeDto.StartDate,
            EndDate = subscribeDto.EndDate,
            SoftwareId = subscribeDto.SoftwareId,
            AdditionalSupportYears = additionalSupportYears,
            Price = subscribeDto.RenewalPrice
        };

        await _contractService.CreateContractAsync(createContractDto);

        int contractId = await _contractService.GetContractIdAsync(createContractDto);


        //tworzymy pierwszą płatność za subscrybcje (oraz nakładamy aktywne zniżki)

        Discount discount = await _discountService.GetBiggestDiscountAsync();

        decimal firstPaymentValue = subscribeDto.RenewalPrice -= discount.Value;

        //sprawdzamy, czy klient ma już umowę na to oprogramowanie

        if (await _contractService.ClientHasContractForSoftwareAsync(subscribeDto.ClientId, subscribeDto.SoftwareId))
        {
            throw new ArgumentException("Client already has contract for this software");
        }

        //sprawdzamy, czy klient ma już umowę w firmie (jeśli tak dajemy 5% zniżki)

        if (await _contractService.ClientHasContractForAnySoftwareAsync(subscribeDto.ClientId))
        {
            firstPaymentValue *= 0.95m;
        }

        await _contractService.CreatePaymentAsync(firstPaymentValue, contractId);
    }


    public async Task<List<GetSubscriptionDto>> GetSubscriptionsListAsync()
    {
        return await _subscriptionRepository.GetSubscriptionsList();
    }

    public Task PayForSubscriptionAsync(PaymentDto payForSubscriptionDto)
    {
        throw new NotImplementedException();
    }
}