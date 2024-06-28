using abcAPI.Exceptions;
using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;
using abcAPI.Repositories;

namespace abcAPI.Services;

public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;
    private readonly IDiscountService _discountService;
    private readonly IClientService _clientService;


    public ContractService(IContractRepository contractRepository, IDiscountService discountService,
        IClientService clientService)
    {
        _contractRepository = contractRepository;
        _discountService = discountService;
        _clientService = clientService;
    }

    public async Task CreateContractAsync(CreateContractDto createContractDto, bool fromSubscription)
    {
        await _clientService.GetClientAsync(createContractDto.ClientId);

        if (!fromSubscription)
        {
            if (createContractDto.StartDate >= createContractDto.EndDate)
            {
                throw new ArgumentException("Start date must be before end date");
            }

            if ((createContractDto.EndDate - createContractDto.StartDate).TotalDays < 3 ||
                (createContractDto.EndDate - createContractDto.StartDate).TotalDays > 30)
            {
                throw new ArgumentException("Contract must last between 3 and 30 days");
            }

            if (createContractDto.AdditionalSupportYears < 1 || createContractDto.AdditionalSupportYears > 3)
            {
                throw new ArgumentException("Additional support years must be between 1 and 3");
            }
        }

        bool contractExists = await
            _contractRepository.ClientHasContractForSoftwareAsync(createContractDto.ClientId,
                createContractDto.SoftwareId);

        if (contractExists)
        {
            throw new ArgumentException("Client already has contract for this software");
        }

        //obliczenie ceny umowy pierwszy rok jest za darmo a każdy kolejny kosztuje 1000

        decimal price = createContractDto.Price + (createContractDto.AdditionalSupportYears - 1) * 1000;

        //nie do końca zrozumiałem polecenie gdyż napisane jest że cena obejmuje wszystkie zniżki
        // a zaraz potem że Gdy dostępnych jest wiele zniżek, wybieramy najwyższą.
        //dlatego też założyłem że automatycznie obniżę cenę o największą dostępną zniżkę
        try
        {
            Discount? discount = await _discountService.GetBiggestDiscountAsync();
            price = price - discount.Value;
        }
        catch (NotFoundException e)
        {
            //ignore
        }

        if (await _contractRepository.ClientHasContractForAnySoftwareAsync(createContractDto.ClientId))
        {
            price *= 0.95m;
        }

        Contract contract = new()
        {
            SoftwareId = createContractDto.SoftwareId,
            StartDate = createContractDto.StartDate,
            EndDate = createContractDto.EndDate,
            Price = price,
            Version = createContractDto.Version,
            AdditionalSupportYears = createContractDto.AdditionalSupportYears
        };

        await _contractRepository.CreateContractAsync(contract, createContractDto.ClientId);
    }

    public async Task PayForContractAsync(PaymentDto paymentDto)
    {
        Contract? contract = await _contractRepository.GetContractByIdAsync(paymentDto.ContractId);
        if (contract == null || contract.EndDate < DateTime.Now)
        {
            throw new NotFoundException("Invalid contract or contract has expired.");
        }

        contract.AmountPaid += paymentDto.Amount;
        if (contract.AmountPaid >= contract.Price)
        {
            contract.IsPaid = true;
            contract.IsSigned = true;
        }

        await _contractRepository.UpdateContractAsync(contract);

        Payment payment = new Payment
        {
            Amount = paymentDto.Amount,
            ContractId = paymentDto.ContractId,
            Date = DateTime.Now
        };

        await _contractRepository.AddPaymentAsync(payment);
    }


    public async Task<List<GetContractDto>> GetContractsAsync()
    {
        return await _contractRepository.GetContractsAsync();
    }

    public async Task SignContractAsync(int contractId)
    {
        await _contractRepository.SignContractAsync(contractId);
    }

    public async Task DeleteContractAsync(int contractId)
    {
        await _contractRepository.DeleteContractAsync(contractId);
    }

    public async Task<Contract> GetContractByIdAsync(int contractId)
    {
        return await _contractRepository.GetContractByIdAsync(contractId);
    }

    public async Task CreatePaymentAsync(decimal price, int contractId)
    {
        Payment payment = new Payment
        {
            Amount = price,
            ContractId = contractId,
            Date = DateTime.Now
        };

        await _contractRepository.AddPaymentAsync(payment);
    }

    public async Task<int> GetContractIdAsync(CreateContractDto createContractDto)
    {
        return await _contractRepository.GetContractIdAsync(createContractDto);
    }

    public async Task<bool> ClientHasContractForSoftwareAsync(int clientId, int softwareId)
    {
        return await _contractRepository.ClientHasContractForSoftwareAsync(clientId, softwareId);
    }

    public async Task<bool> ClientHasContractForAnySoftwareAsync(int subscribeDtoClientId)
    {
        return await _contractRepository.ClientHasContractForAnySoftwareAsync(subscribeDtoClientId);
    }

    public async Task<bool> ClientHasPaidForSubscriptionAsync(int contractId, bool isMonthly)
    {
        return await _contractRepository.ClientHasPaidForSubscriptionAsync(contractId, isMonthly);
    }



    public async Task<List<PaymentDto>> GetPaymentsForContract(GetContractDto contract)
    {
        return await _contractRepository.GetPaymentsForContract(contract);
    }
}