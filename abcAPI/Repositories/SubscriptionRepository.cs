using abcAPI.Models;
using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;

namespace abcAPI.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private AppDbContext _context;

    public SubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task Subscribe(SubscribeDto subscribeDto)
    {
        throw new NotImplementedException();
    }

    public async Task Unsubscribe(SubscribeDto subscribeDto)
    {
        throw new NotImplementedException();
    }

    public async Task<List<GetSubscriptionDto>> GetSubscriptionsList()
    {
        throw new NotImplementedException();
    }
}