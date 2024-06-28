namespace abcAPI.Models.TableModels;

public class Subscription
{
    public int Id { get; set; }
    public string OfferName { get; set; }

    public int ContractId { get; set; }
    public Contract Contract { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal PriceOfRenewal { get; set; }
    public string RenewalPeriod { get; set; } // e.g., Monthly, Yearly
    public bool IsActive { get; set; }
}