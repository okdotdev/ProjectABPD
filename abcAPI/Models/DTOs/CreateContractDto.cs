namespace abcAPI.Models.DTOs;

public class CreateContractDto
{
    public int ClientId { get; set; }
    public int SoftwareId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public int AdditionalSupportYears { get; set; }
}