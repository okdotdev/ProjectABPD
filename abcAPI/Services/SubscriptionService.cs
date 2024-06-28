using abcAPI.Models.DTOs;
using abcAPI.Repositories;

namespace abcAPI.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task SubscribeAsync(SubscribeDto subscribeDto)
    {
        await _subscriptionRepository.Subscribe(subscribeDto);
    }

    public async Task UnsubscribeAsync(SubscribeDto subscribeDto)
    {
        await _subscriptionRepository.Unsubscribe(subscribeDto);
    }

    public async Task<List<GetSubscriptionDto>> GetSubscriptionsListAsync()
    {
        return await _subscriptionRepository.GetSubscriptionsList();
    }
}