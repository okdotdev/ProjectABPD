namespace abcAPI.Models.DTOs;

public class GetClientDto
{
    public int IdClient { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}

public class GetClientIndividualDto : GetClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Pesel { get; set; }
}

public class GetClientCompanyDto : GetClientDto
{
    public string CompanyName { get; set; }
    public string Krs { get; set; }
}