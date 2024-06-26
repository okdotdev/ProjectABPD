
using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;

namespace abcAPI.Services;

public interface IContractService
{
    Task<Contract> CreateContractAsync(CreateContractDto createContractDto);
    Task<bool> PayForContractAsync(int contractId, PaymentDto paymentDto);
    Task<List<GetContractDto>> GetContractsAsync();
    Task SignContractAsync(int contractId);
    Task DeleteContractAsync(int contractId);
}