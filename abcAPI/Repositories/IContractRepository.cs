using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;

namespace abcAPI.Repositories;

public interface IContractRepository
{

    Task<List<GetContractDto>> GetContractsAsync();
    Task SignContractAsync(int contractId);
    Task DeleteContractAsync(int contractId);
    bool ClientHasContractForSoftwareAsync(int clientId, int softwareId);
    bool ClientHasContractForAnySoftwareAsync(int clientId);
    Task CreateContractAsync(Contract contract, int clientId);
    Task<Contract> GetContractByIdAsync(int contractId);
    Task UpdateContractAsync(Contract contract);
    Task AddPaymentAsync(Payment payment);
}