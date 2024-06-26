namespace abcAPI.Models.DTOs;

public class GetContractDto
{
    public int CustomerId { get; set; }
    public int Id { get; set; }
    public int SoftwareId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public bool IsPaid { get; set; }
    public string Version { get; set; }
    public int AdditionalSupportYears { get; set; }
    public bool IsSigned { get; set; }
}