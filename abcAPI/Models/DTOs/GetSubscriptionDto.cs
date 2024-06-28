namespace abcAPI.Models.DTOs;

public class GetSubscriptionDto
{
    public int Id { get; set; }
    public int SoftwareId { get; set; }
    public int ClientId { get; set; }
    public int ContractId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsMonthly { get; set; }
    public decimal Price { get; set; }
    public decimal RenewalPrice { get; set; }
}