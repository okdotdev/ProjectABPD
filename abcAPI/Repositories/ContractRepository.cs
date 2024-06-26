using abcAPI.Exceptions;
using abcAPI.Models;
using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;
using Microsoft.EntityFrameworkCore;

namespace abcAPI.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly AppDbContext _context;

    public ContractRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Contract> GetContractByIdAsync(int contractId)
    {
        Contract? contract = await _context.Contracts.FindAsync(contractId);

        if (contract == null)
        {
            throw new NotFoundException("Contract not found");
        }

        return contract;
    }

    public async Task<IEnumerable<Contract>> GetActiveContractsForClientAsync(int clientId, int softwareId)
    {
        return await _context.Contracts
            .Where(c => c.ClientContracts.Any(cc => cc.ClientId == clientId) && c.SoftwareId == softwareId &&
                        c.IsSigned && c.EndDate >= DateTime.Now)
            .ToListAsync();
    }

    public async Task AddContractAsync(Contract contract)
    {
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateContractAsync(Contract contract)
    {
        _context.Contracts.Update(contract);
        await _context.SaveChangesAsync();
    }

    public async Task<List<GetContractDto>> GetContractsAsync()
    {
        DateTime currentDate = DateTime.Now;
        List<Contract> contracts = await _context.Contracts.ToListAsync();
        List<ClientContract> clientContracts = await _context.ClientContracts.ToListAsync();


        //w tym miejscu usuwam nie opłacone umowy które już się skończyły
        List<Contract> contractsToDelete = contracts.Where(c => !c.IsPaid && c.EndDate <= currentDate).ToList();

        if (contractsToDelete.Any())
        {
            _context.Contracts.RemoveRange(contractsToDelete);
            await _context.SaveChangesAsync();
        }

        List<GetContractDto> contractDtos = contracts
            .Where(c => !contractsToDelete.Contains(c))
            .Select(contract => new GetContractDto
            {
                Id = contract.Id,
                CustomerId = clientContracts.First(cc => cc.ContractId == contract.Id).ClientId,
                SoftwareId = contract.SoftwareId,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                Price = contract.Price,
                IsPaid = contract.IsPaid,
                Version = contract.Version,
                AdditionalSupportYears = contract.AdditionalSupportYears,
                IsSigned = contract.IsSigned
            }).ToList();

        return contractDtos;
    }

    public async Task SignContractAsync(int contractId)
    {
        //null check nie jest potrzebny bo GetContractByIdAsync zwróci NotFoundException
        Contract contract = await GetContractByIdAsync(contractId);

        contract.IsSigned = true;

        await UpdateContractAsync(contract);
    }

    public async Task DeleteContractAsync(int contractId)
    {
        Contract? contract = await _context.Contracts.FindAsync(contractId);
        if (contract != null)
        {
            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new NotFoundException("Contract not found");
        }
    }
}