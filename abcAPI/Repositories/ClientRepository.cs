using abcAPI.Exceptions;
using abcAPI.Models;
using abcAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;


namespace abcAPI.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddClientAsync(AddClientDto client)
    {
        Client newClient = client switch
        {
            AddClientIndividualDto clientIndividual => new ClientIndividual
            {
                Email = clientIndividual.Email, Address = clientIndividual.Address,
                PhoneNumber = clientIndividual.PhoneNumber, FirstName = clientIndividual.FirstName,
                LastName = clientIndividual.LastName, Pesel = clientIndividual.Pesel
            },
            AddClientCompanyDto clientCompany => new ClientCompany
            {
                Email = clientCompany.Email, Address = clientCompany.Address,
                PhoneNumber = clientCompany.PhoneNumber, CompanyName = clientCompany.CompanyName,
                Krs = clientCompany.Krs
            },
            _ => throw new ArgumentException("Invalid client type")
        };

        await _context.Clients.AddAsync(newClient);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateClientAsync(UpdateClientDto client)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteClientAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<GetClientDto>> GetClientsListAsync(string type)
    {
        List<GetClientDto> clientDtos;

        switch (type.ToLower())
        {
            case "individual":
                List<ClientIndividual> individualClients = await _context.Clients.OfType<ClientIndividual>().ToListAsync();
                clientDtos = individualClients.Select(client => new GetClientIndividualDto
                {
                    IdClient = client.IdClient,
                    Address = client.Address,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Pesel = client.Pesel
                }).ToList<GetClientDto>();
                break;

            case "company":
                List<ClientCompany> companyClients = await _context.Clients.OfType<ClientCompany>().ToListAsync();
                clientDtos = companyClients.Select(client => new GetClientCompanyDto
                {
                    IdClient = client.IdClient,
                    Address = client.Address,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber,
                    CompanyName = client.CompanyName,
                    Krs = client.Krs
                }).ToList<GetClientDto>();
                break;

            default:
                throw new ArgumentException("Invalid client type");
        }

        return clientDtos;
    }
}