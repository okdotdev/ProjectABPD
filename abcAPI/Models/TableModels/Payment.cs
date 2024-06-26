namespace abcAPI.Models.TableModels;

public class Payment
{
    public int Id { get; set; }
    public int ContractId { get; set; }
    public Contract Contract { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
}