using abcAPI.Models;
using abcAPI.Models.DTOs;
using abcAPI.Models.ViewModels;

namespace abcAPI.Services;

public interface IClientService
{
    Task AddClientAsync(AddClientDto client);
    Task UpdateClientAsync(UpdateClientDto client, string nickname);
    Task DeleteClientAsync(int id, string nickname);
    Task<List<GetClientDto>> GetClientsListAsync(string type);
    Task GetClientAsync(int clientId);
}