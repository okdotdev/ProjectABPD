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

        if (subscribeDto.StartDate > subscribeDto.EndDate)
        {
            throw new ArgumentException("End date should be greater than start date");
        }

        switch (subscribeDto.EndDate.Subtract(subscribeDto.StartDate).TotalDays)
        {
            case < 30:
                throw new ArgumentException("Subscription time should be at least 1 month");
            case > 730:
                throw new ArgumentException("Subscription time should be at most 2 years");
            default:
                await _subscriptionRepository.Subscribe(subscribeDto);
                break;
        }


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