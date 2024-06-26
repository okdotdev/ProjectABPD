namespace abcAPI.Models.DTOs;

public class RevenueResponseDto
{
    public decimal CurrentRevenue { get; set; }
    public decimal ProjectedRevenue { get; set; }
    public string Currency { get; set; }
}