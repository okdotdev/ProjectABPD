using abcAPI.Models.TableModels;

namespace abcAPI.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    public Task<IEnumerable<Subscription>> GetActiveSubscriptionsForClientAsync(int clientIdClient, int softwareId)
    {
        throw new NotImplementedException();
    }

    public bool IsLoyalCustomer(int clientIdClient)
    {
        throw new NotImplementedException();
    }
}