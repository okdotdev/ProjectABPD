using abcAPI.Models.DTOs;

namespace abcAPI.Services;

public interface IRevenueService
{
    Task<RevenueResponseDto> CalculateRevenueAsync(RevenueRequestDto requestDto);
}