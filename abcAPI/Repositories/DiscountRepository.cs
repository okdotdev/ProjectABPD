using abcAPI.Exceptions;
using abcAPI.Models;
using abcAPI.Models.TableModels;

namespace abcAPI.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly AppDbContext _context;

    public DiscountRepository(AppDbContext context)
    {
        _context = context;
    }


    public Task<Discount> GetBiggestDiscountAsync()
    {
        List<Discount> discounts = _context.Discounts
            .Where(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
            .OrderByDescending(d => d.Value)
            .ToList();


        Discount? biggestDiscount = discounts.FirstOrDefault();

        if (biggestDiscount == null)
        {
            throw new NotFoundException("No active discounts found");
        }

        return Task.FromResult(biggestDiscount);
    }
}