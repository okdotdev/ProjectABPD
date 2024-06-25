using abcAPI.Exceptions;
using abcAPI.Models;
using abcAPI.Models.DTOs;
using abcAPI.Models.ViewModels;
using abcAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace abcAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : Controller
{
    private readonly IClientService _clientService;
    private readonly UserManager<User> _userManager;


    public ClientController(IClientService clientService, UserManager<User> userManager)
    {
        _clientService = clientService;
        _userManager = userManager;
    }

    [HttpPost("clients/add")]
    public async Task<IActionResult> AddClient(AddClientDto client)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            await _clientService.AddClientAsync(client);
            return Ok("Client Added Successfully");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }


    [HttpPut("clients/update/{id}")]
    public async Task<IActionResult> UpdateClient(UpdateClientDto client)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            await _clientService.UpdateClientAsync(client, user.Nickname);
            return Ok("Customer Updated Successfully");
        }
        catch (AccessDeniedException e)
        {
            return StatusCode(403, e.Message);
        }
        catch (ClientNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("clients/delete/{id:int}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            await _clientService.DeleteClientAsync(id, user.Nickname);
            return Ok("Customer Deleted Successfully");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("clients/list/{type}")]
    public async Task<IActionResult> GetClientsList(string type)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            List<GetClientDto> clientsList = await _clientService.GetClientsListAsync(type);
            return Ok(clientsList);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("corporate-clients")]
    public async Task<IActionResult> CorporateClients()
    {
        var user = await _userManager.GetUserAsync(User);

        List<GetClientDto> clients = await _clientService.GetClientsListAsync("company");
        ClientViewModel model = new ClientViewModel();
        List<ClientCompany> clientCompanies = (from GetClientCompanyDto client in clients
            select new ClientCompany()
            {
                IdClient = client.IdClient,
                Address = client.Address,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                CompanyName = client.CompanyName,
                Krs = client.Krs
            }).ToList();

        model.CorporateClients = clientCompanies;
        return View(model);
    }

    [HttpGet("individual-clients")]
    public async Task<IActionResult> IndividualClients()
    {
        var user = await _userManager.GetUserAsync(User);

        List<GetClientDto> clients = await _clientService.GetClientsListAsync("individual");
        ClientViewModel model = new ClientViewModel();
        List<ClientIndividual> clientIndividuals = (from GetClientIndividualDto client in clients
            select new ClientIndividual()
            {
                IdClient = client.IdClient,
                Address = client.Address,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Pesel = client.Pesel
            }).ToList();


        return View(model);
    }
}