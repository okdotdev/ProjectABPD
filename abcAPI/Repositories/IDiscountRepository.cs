using abcAPI.Models.TableModels;

namespace abcAPI.Repositories;

public interface IDiscountRepository
{
    Task<Discount> GetBiggestDiscountAsync();
}