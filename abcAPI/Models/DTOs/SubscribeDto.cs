namespace abcAPI.Models.DTOs;

public class SubscribeDto
{
    public string OfferName { get; set; }
    public int SoftwareId { get; set; }
    public int ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsMonthly { get; set; } // false = yearly
    public decimal RenewalPrice { get; set; } // for full period
}