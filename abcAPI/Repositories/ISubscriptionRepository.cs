using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;

namespace abcAPI.Repositories;

public interface ISubscriptionRepository
{
    Task Subscribe(SubscribeDto subscribeDto, int contractId);
    Task<List<GetSubscriptionDto>> GetSubscriptionsList();
}