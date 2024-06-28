using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;

namespace abcAPI.Repositories;

public interface ISubscriptionRepository
{
    Task Subscribe(SubscribeDto subscribeDto);
    Task<List<GetSubscriptionDto>> GetSubscriptionsList();
}