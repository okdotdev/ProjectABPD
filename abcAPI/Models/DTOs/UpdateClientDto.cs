namespace abcAPI.Models.DTOs;

public class UpdateClientDto
{
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}

public class UpdateClientIndividualDto : UpdateClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

}

public class UpdateClientCompanyDto : UpdateClientDto
{
    public string CompanyName { get; set; }
}