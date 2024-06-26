using abcAPI.Models;
using abcAPI.Models.DTOs;


namespace abcAPI.Repositories;

public interface IClientRepository
{
    Task AddClientAsync(AddClientDto client);
    Task UpdateClientAsync(UpdateClientDto client);
    Task DeleteClientAsync(int id);
    Task<Client> GetClientByIdAsync(int clientId);
    Task<List<GetClientDto>> GetClientsListAsync(string type);

}