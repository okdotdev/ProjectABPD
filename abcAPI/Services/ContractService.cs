using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;
using abcAPI.Models.DTOs;
using abcAPI.Repositories;

namespace abcAPI.Services;

public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;


    public ContractService(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;
    }


    public async Task<Contract> CreateContractAsync(CreateContractDto createContractDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> PayForContractAsync(int contractId, PaymentDto paymentDto)
    {
        throw new NotImplementedException();
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
}