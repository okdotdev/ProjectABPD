using abcAPI.Models.DTOs;

namespace abcAPI.Services;

public interface IRevenueService
{
    Task<decimal> CalculateProjectedRevenueAsync(RevenueRequestDto requestDto);
    Task<decimal> CalculateRealRevenueAsync(RevenueRequestDto requestDto);
}