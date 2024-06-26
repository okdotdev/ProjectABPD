using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;

namespace abcAPI.Repositories;

public interface IContractRepository
{
    Task<Contract> GetContractByIdAsync(int contractId);
    Task<IEnumerable<Contract>> GetActiveContractsForClientAsync(int clientId, int softwareId);
    Task AddContractAsync(Contract contract);
    Task UpdateContractAsync(Contract contract);
    Task<List<GetContractDto>> GetContractsAsync();
    Task SignContractAsync(int contractId);
    Task DeleteContractAsync(int contractId);
}