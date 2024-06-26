using abcAPI.Models.TableModels;
using abcAPI.Repositories;

namespace abcAPI.Services;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;

    public DiscountService(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<Discount> GetBiggestDiscountAsync()
    {
       return   await _discountRepository.GetBiggestDiscountAsync();
    }
}