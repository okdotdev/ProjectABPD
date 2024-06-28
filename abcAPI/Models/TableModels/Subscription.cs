namespace abcAPI.Models.TableModels;

public class Subscription
{
    public int Id { get; set; }
    public string OfferName { get; set; }

    public int ContractId { get; set; }
    public Contract Contract { get; set; }
    public decimal PriceOfRenewal { get; set; }
    public bool IsMonthly { get; set; } //false = yearly
    public bool IsActive { get; set; }
}