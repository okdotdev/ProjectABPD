using abcAPI.Models.DTOs;

namespace abcAPI.Services;

public interface ISubscriptionService
{
    Task SubscribeAsync(SubscribeDto subscribeDto);
    Task UnsubscribeAsync(SubscribeDto subscribeDto);
    Task<List<GetSubscriptionDto>> GetSubscriptionsListAsync();
}