namespace abcAPI.Models.TableModels;

public class Subscription
{
    public int Id { get; set; }
    public int SoftwareId { get; set; }
    public Software Software { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public string RenewalPeriod { get; set; } // e.g., Monthly, Yearly
    public bool IsActive { get; set; }
}