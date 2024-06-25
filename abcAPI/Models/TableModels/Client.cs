namespace abcAPI.Models;

public  class Client
{
    public int IdClient { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsDeleted { get; set; } = false;

}

public class ClientIndividual : Client
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Pesel { get; set; }
}

public class ClientCompany : Client
{
    public string CompanyName { get; set; }
    public string Krs { get; set; }
}