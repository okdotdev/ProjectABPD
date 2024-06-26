using abcAPI.Models.TableModels;

namespace abcAPI.Repositories;

public interface IDiscountRepository
{
    Task<IEnumerable<Discount>> GetActiveDiscountsAsync();
}