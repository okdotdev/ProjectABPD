namespace abcAPI.Models.TableModels;

public class ClientContract //Transaction
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public int ContractId { get; set; }
    public Contract Contract { get; set; }
}