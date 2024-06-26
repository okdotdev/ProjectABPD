namespace abcAPI.Models.TableModels;

public class Contract
{
    public int Id { get; set; }
    public int SoftwareId { get; set; }
    public Software Software { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public bool IsPaid { get; set; }
    public string Version { get; set; }
    public int AdditionalSupportYears { get; set; }
    public bool IsSigned { get; set; }
    public List<ClientContract> ClientContracts { get; set; }
}