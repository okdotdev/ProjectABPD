using abcAPI.Exceptions;
using abcAPI.Models;
using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;
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

    [HttpPost("clients/add/individual")]
    public async Task<IActionResult> AddIndividualClient([FromForm] AddClientIndividualDto client)
    {
        if (!IsUserLoggedIn())
        {
            return BadRequest("User not logged in");
        }

        try
        {
            await _clientService.AddClientAsync(client);
            return RedirectToAction("IndividualClients");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("clients/add/corporate")]
    public async Task<IActionResult> AddCorporateClient([FromForm] AddClientCompanyDto client)
    {
        if (!IsUserLoggedIn())
        {
            return BadRequest("User not logged in");
        }

        try
        {
            await _clientService.AddClientAsync(client);
            return RedirectToAction("CorporateClients");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("clients/update/individual")]
    public async Task<IActionResult> UpdateIndividualClient([FromForm] UpdateClientIndividualDto client)
    {
        User? user = await _userManager.GetUserAsync(User);


        if (user == null)
        {
            return BadRequest("User not logged in");
        }

        try
        {
            await _clientService.UpdateClientAsync(client, user.UserName);
            return RedirectToAction("IndividualClients");
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

    [HttpPost("clients/update/corporate")]
    public async Task<IActionResult> UpdateCorporateClient([FromForm] UpdateClientCompanyDto client)
    {
        User? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return BadRequest("User not logged in");
        }

        try
        {
            await _clientService.UpdateClientAsync(client, user.UserName);
            return RedirectToAction("CorporateClients");
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


    //Form nie obsługuje delete więc trzeba zrobić posta
    [HttpPost("clients/delete/individual")]
    public async Task<IActionResult> DeleteIndividualClient([FromForm] int IdClient)
    {
        User? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return BadRequest("User not logged in");
        }

        try
        {
            await _clientService.DeleteClientAsync(IdClient, user.UserName);
            return RedirectToAction("IndividualClients");
        }
        catch (CantDeleteCompanyException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("clients/list/{type}")]
    public async Task<IActionResult> GetClientsList(string type)
    {
        if (!IsUserLoggedIn())
        {
            return BadRequest("User not logged in");
        }

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
        if (!IsUserLoggedIn())
        {
            return BadRequest("User not logged in");
        }

        List<GetClientDto> clients = await _clientService.GetClientsListAsync("company");
        ClientViewModel model = new()
        {
            CorporateClients = clients.Cast<GetClientCompanyDto>().Select(client => new ClientCompany
            {
                IdClient = client.IdClient,
                Address = client.Address,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                CompanyName = client.CompanyName,
                Krs = client.Krs
            }).ToList(),
        };

        return View(model);
    }

    [HttpGet("individual-clients")]
    public async Task<IActionResult> IndividualClients()
    {
        if (!IsUserLoggedIn())
        {
            return BadRequest("User not logged in");
        }

        List<GetClientDto> clients = await _clientService.GetClientsListAsync("individual");
        ClientViewModel model = new()
        {
            IndividualClients = clients.Cast<GetClientIndividualDto>().Select(client => new ClientIndividual
            {
                IdClient = client.IdClient,
                Address = client.Address,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Pesel = client.Pesel
            }).ToList(),
        };

        return View(model);
    }

    public bool IsUserLoggedIn()
    {
        return _userManager.GetUserAsync(User).Result != null;
    }
}