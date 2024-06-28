using abcAPI.Exceptions;
using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;
using abcAPI.Repositories;

namespace abcAPI.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IContractService _contractService;
    private readonly IDiscountService _discountService;
    private readonly ISoftwareService _softwareService;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository, IContractService contractService,
        IDiscountService discountService, ISoftwareService softwareService)
    {
        _subscriptionRepository = subscriptionRepository;
        _contractService = contractService;
        _discountService = discountService;
        _softwareService = softwareService;
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
        }

        //tworzymy kontrakt

        int additionalSupportYears = subscribeDto.EndDate.Year - subscribeDto.StartDate.Year;

        //get software version

        GetSoftwareDto software = await _softwareService.GetSoftwareAsync(subscribeDto.SoftwareId);


        CreateContractDto createContractDto = new()
        {
            ClientId = subscribeDto.ClientId,
            StartDate = subscribeDto.StartDate,
            EndDate = subscribeDto.EndDate,
            SoftwareId = subscribeDto.SoftwareId,
            AdditionalSupportYears = additionalSupportYears,
            Version = software.CurrentVersion,
            Price = subscribeDto.RenewalPrice
        };

        await _contractService.CreateContractAsync(createContractDto, true);

        int contractId = await _contractService.GetContractIdAsync(createContractDto);


        //tworzymy pierwszą płatność za subscrybcje (oraz nakładamy aktywne zniżki)
        Discount discount;
        try
        {
            discount = await _discountService.GetBiggestDiscountAsync();
        }
        catch (NotFoundException)
        {
            discount = new Discount { Value = 0 };
        }


        decimal firstPaymentValue = subscribeDto.RenewalPrice -= discount.Value;

        if (firstPaymentValue < 0)
        {
            firstPaymentValue = 0;
        }

        //sprawdzamy, czy klient ma już umowę na to oprogramowanie


        if (await _contractService.ClientHasContractForSoftwareAsync(subscribeDto.ClientId, subscribeDto.SoftwareId))
        {
            throw new ArgumentException("Client already has contract for this software");
        }

        //sprawdzamy, czy klient ma już umowę w firmie (jeśli tak dajemy 5% zniżki)

        if (await _contractService.ClientHasContractForAnySoftwareAsync(subscribeDto.ClientId))
        {
            firstPaymentValue *= 0.95m;
            subscribeDto.RenewalPrice *= 0.95m;
        }

        await _contractService.CreatePaymentAsync(firstPaymentValue, contractId);

        //tworzymy subskrypcję

        await _subscriptionRepository.Subscribe(subscribeDto, contractId);
    }


    public async Task<List<GetSubscriptionDto>> GetSubscriptionsListAsync()
    {
        return await _subscriptionRepository.GetSubscriptionsList();
    }

    public async Task PayForSubscriptionAsync(PaymentDto payForSubscriptionDto)
    {
        Subscription subscription = await _subscriptionRepository.GetSubscription(payForSubscriptionDto.ContractId);

        if (payForSubscriptionDto.Amount != subscription.PriceOfRenewal)
        {
            throw new ArgumentException("Payment must equal renewal price");
        }

        //zniżka dla lojalnych klientów jest już nałożona na cenę subskrypcji

        //sprawdźmy czy client już zapłacił za tą subskrypcję

        if (await _contractService.ClientHasPaidForSubscriptionAsync(payForSubscriptionDto.ContractId,
                subscription.IsMonthly))
        {
            throw new ArgumentException("Client has already paid for this subscription");
        }

        await _contractService.CreatePaymentAsync(payForSubscriptionDto.Amount, payForSubscriptionDto.ContractId);
    }
}