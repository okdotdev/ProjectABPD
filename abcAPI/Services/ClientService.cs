using abcAPI.Exceptions;
using abcAPI.Models;
using abcAPI.Models.DTOs;
using abcAPI.Models.ViewModels;
using abcAPI.Repositories;
using Microsoft.AspNetCore.Identity;

namespace abcAPI.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;
    private readonly UserManager<User> _userManager;


    public ClientService(IClientRepository repository, UserManager<User> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }



    public async Task AddClientAsync(AddClientDto client)
    {
        await _repository.AddClientAsync(client);
    }

    public async Task UpdateClientAsync(UpdateClientDto client ,string nickname)
    {
        User? user = await _userManager.FindByNameAsync(nickname);
        if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
        {
            throw new AccessDeniedException("User is not an Admin");
        }

        await _repository.UpdateClientAsync(client);
    }

    public async Task DeleteClientAsync(int id,string nickname)
    {
        User? user = await _userManager.FindByNameAsync(nickname);
        if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
        {
            throw new AccessDeniedException("User is not an Admin");
        }

        await _repository.DeleteClientAsync(id);
    }

    public async Task<List<GetClientDto>> GetClientsListAsync(string type)
    {
        return await _repository.GetClientsListAsync(type);
    }
}