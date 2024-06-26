
using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;

namespace abcAPI.Services;

public interface IContractService
{
    Task CreateContractAsync(CreateContractDto createContractDto);
    Task PayForContractAsync( PaymentDto paymentDto);
    Task<List<GetContractDto>> GetContractsAsync();
    Task SignContractAsync(int contractId);
    Task DeleteContractAsync(int contractId);
    Task<Contract> GetContractByIdAsync(int contractId);
}