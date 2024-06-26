using abcAPI.Models;
using abcAPI.Models.TableModels;
using Microsoft.EntityFrameworkCore;

namespace abcAPI.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly AppDbContext _context;

    public DiscountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Discount>> GetActiveDiscountsAsync()
    {
        var currentDate = DateTime.Now;
        return await _context.Discounts
            .Where(d => d.StartDate <= currentDate && d.EndDate >= currentDate)
            .ToListAsync();
    }
}