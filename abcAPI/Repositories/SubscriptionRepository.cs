using abcAPI.Models;
using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;
using Microsoft.EntityFrameworkCore;

namespace abcAPI.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private AppDbContext _context;

    public SubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task Subscribe(SubscribeDto subscribeDto, int contractId)
    {
        Subscription subscription = new()
        {
            OfferName = subscribeDto.OfferName,
            ContractId = contractId,
            IsMonthly = subscribeDto.IsMonthly,
            PriceOfRenewal = subscribeDto.RenewalPrice,
            IsActive = true
        };

        await _context.Subscriptions.AddAsync(subscription);
        await _context.SaveChangesAsync();
    }


    public async Task<List<GetSubscriptionDto>> GetSubscriptionsList()
    {
        List<Subscription> subscriptions = await _context.Subscriptions.ToListAsync();

        foreach (Subscription subscription in subscriptions)
        {
            List<Payment> payments =
                await _context.Payments.Where(p => p.ContractId == subscription.ContractId).ToListAsync();

            Contract? contract = await _context.Contracts.FirstOrDefaultAsync(c => c.Id == subscription.ContractId);

            if (contract == null)
            {
                throw new Exception("Subscription has no contract");
            }

            Payment? lastPayment = payments.MaxBy(p => p.Date);

            DateTime startDate = contract.StartDate;


            TimeSpan renewalPeriod = subscription.IsMonthly ? TimeSpan.FromDays(30) : TimeSpan.FromDays(365);


            if (lastPayment == null)
            {
                if (DateTime.Now - startDate <= renewalPeriod) continue;
                _context.Subscriptions.Remove(subscription);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (DateTime.Now - lastPayment.Date <= renewalPeriod) continue;
                _context.Subscriptions.Remove(subscription);
                await _context.SaveChangesAsync();
            }
        }

        List<GetSubscriptionDto> getSubscriptionDtos = subscriptions.Select(s => new GetSubscriptionDto
        {
            Id = s.Id,
            ContractId = s.ContractId,
            SoftwareId = s.Contract.SoftwareId,
            IsMonthly = s.IsMonthly,
            StartDate = s.Contract.StartDate,
            EndDate = s.Contract.EndDate,
            Price = s.Contract.Price,
            RenewalPrice = s.PriceOfRenewal,
        }).ToList();


        return getSubscriptionDtos;
    }
}