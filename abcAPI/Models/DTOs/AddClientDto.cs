namespace abcAPI.Models.DTOs;

public class AddClientDto
{
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}

public class AddClientIndividualDto : AddClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Pesel { get; set; }
}

public class AddClientCompanyDto : AddClientDto
{
    public string CompanyName { get; set; }
    public string Krs { get; set; }
}