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


    public async Task UpdateContractAsync(Contract contract)
    {
        _context.Contracts.Update(contract);
        await _context.SaveChangesAsync();
    }

    public async Task AddPaymentAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetContractIdAsync(CreateContractDto createContractDto)
    {
        Contract? contract = await _context.Contracts.FirstOrDefaultAsync(c =>
            c.SoftwareId == createContractDto.SoftwareId &&
            c.ClientContracts.Any(cc => cc.ClientId == createContractDto.ClientId));

        if (contract == null)
        {
            throw new NotFoundException("Contract not found");
        }

        return contract.Id;
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
            //usuwam również płatności
            _context.Payments.RemoveRange(_context.Payments.Where(p => contractsToDelete.Contains(p.Contract)));
        }


        foreach (Contract contract in contracts)
        {
            List<Payment> payments = await _context.Payments.Where(p => p.ContractId == contract.Id).ToListAsync();

            decimal sum = 0;

            foreach (Payment payment in payments)
            {
                sum += payment.Amount;
            }

            contract.AmountPaid = sum;

            if (contract.AmountPaid >= contract.Price)
            {
                contract.IsPaid = true;
            }
            else
            {
                contract.IsPaid = false;
            }
        }


        _context.Contracts.UpdateRange(contracts);

        await _context.SaveChangesAsync();

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
                AmountPaid = contract.AmountPaid,
                Version = contract.Version,
                //SoftwareName = contract.Software.Name,
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

    public async Task<bool> ClientHasContractForSoftwareAsync(int clientId, int softwareId)
    {
        return await _context.Contracts.AnyAsync(c => c.ClientContracts.Any(cc => cc.ClientId == clientId) &&
                                                      c.SoftwareId == softwareId && c.IsSigned && c.EndDate >= DateTime.Now);
    }

    public async Task<bool> ClientHasContractForAnySoftwareAsync(int clientId)
    {
        return await _context.Contracts.AnyAsync(c => c.ClientContracts.Any(cc => cc.ClientId == clientId) && c.IsSigned &&
                                           c.EndDate >= DateTime.Now);
    }

    public async Task CreateContractAsync(Contract contract, int clientId)
    {
        _context.Contracts.Add(contract);

        _context.ClientContracts.Add(new ClientContract
        {
            ClientId = clientId,
            Contract = contract
        });
        await _context.SaveChangesAsync();
    }
}