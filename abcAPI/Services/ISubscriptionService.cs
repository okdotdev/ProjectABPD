using abcAPI.Models.DTOs;

namespace abcAPI.Services;

public interface ISubscriptionService
{
    Task SubscribeAsync(SubscribeDto subscribeDto);

    Task<List<GetSubscriptionDto>> GetSubscriptionsListAsync();
    Task PayForSubscriptionAsync(PaymentDto payForSubscriptionDto);
}