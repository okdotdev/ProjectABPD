using abcAPI.Models.TableModels;

namespace abcAPI.Services;

public interface IDiscountService
{
    Task<Discount> GetBiggestDiscountAsync();
}