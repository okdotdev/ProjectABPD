using abcAPI.Models.TableModels;

namespace abcAPI.Repositories;

public interface ISubscriptionRepository
{
    Task<IEnumerable<Subscription>> GetActiveSubscriptionsForClientAsync(int clientIdClient, int softwareId);
    bool IsLoyalCustomer(int clientIdClient);
}