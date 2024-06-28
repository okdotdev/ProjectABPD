using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;

namespace abcAPI.Services;

public interface IContractService
{
    Task CreateContractAsync(CreateContractDto createContractDto, bool fromSubscription);
    Task PayForContractAsync(PaymentDto paymentDto);
    Task<List<GetContractDto>> GetContractsAsync();
    Task SignContractAsync(int contractId);
    Task DeleteContractAsync(int contractId);
    Task<Contract> GetContractByIdAsync(int contractId);
    Task CreatePaymentAsync(decimal price, int contractId);
    Task<int> GetContractIdAsync(CreateContractDto createContractDto);
    Task<bool> ClientHasContractForSoftwareAsync(int clientId, int softwareId);
    Task<bool> ClientHasContractForAnySoftwareAsync(int subscribeDtoClientId);
    Task<bool> ClientHasPaidForSubscriptionAsync(int contractId, bool isMonthly);
}