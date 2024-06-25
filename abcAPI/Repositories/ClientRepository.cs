using abcAPI.Models;
using abcAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using abcAPI.Exceptions;

namespace abcAPI.Repositories
{
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
                    Email = clientIndividual.Email,
                    Address = clientIndividual.Address,
                    PhoneNumber = clientIndividual.PhoneNumber,
                    FirstName = clientIndividual.FirstName,
                    LastName = clientIndividual.LastName,
                    Pesel = clientIndividual.Pesel
                },
                AddClientCompanyDto clientCompany => new ClientCompany
                {
                    Email = clientCompany.Email,
                    Address = clientCompany.Address,
                    PhoneNumber = clientCompany.PhoneNumber,
                    CompanyName = clientCompany.CompanyName,
                    Krs = clientCompany.Krs
                },
                _ => throw new ArgumentException("Invalid client type")
            };

            await _context.Clients.AddAsync(newClient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClientAsync(UpdateClientDto client)
        {
            var existingClient = await _context.Clients.FindAsync(client.IdClient);
            if (existingClient == null)
            {
                throw new ClientNotFoundException("Client not found");
            }

            switch (existingClient)
            {
                case ClientIndividual individual:
                    var individualUpdate = client as UpdateClientIndividualDto;
                    individual.FirstName = individualUpdate.FirstName;
                    individual.LastName = individualUpdate.LastName;
                    individual.Pesel = individualUpdate.Pesel;
                    break;
                case ClientCompany company:
                    var companyUpdate = client as UpdateClientCompanyDto;
                    company.CompanyName = companyUpdate.CompanyName;
                    company.Krs = companyUpdate.Krs;
                    break;
            }

            existingClient.Address = client.Address;
            existingClient.Email = client.Email;
            existingClient.PhoneNumber = client.PhoneNumber;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                if (client is ClientCompany)
                {
                    throw new CantDeleteCompanyException("Can't delete company client");
                }
                client.IsDeleted = true;

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ClientNotFoundException("Client not found");
            }
        }

        public async Task<List<GetClientDto>> GetClientsListAsync(string type)
        {
            List<GetClientDto> clientDtos;

            switch (type.ToLower())
            {
                case "individual":
                    List<ClientIndividual> individualClients =
                        await _context.Clients.OfType<ClientIndividual>().ToListAsync();
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
}