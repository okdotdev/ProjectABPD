using abcAPI.Exceptions;
using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;
using abcAPI.Models.ViewModels;
using abcAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace abcAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SoftwareController : Controller
{
    private readonly ISoftwareService _softwareService;
    private readonly UserManager<User> _userManager;

    public SoftwareController(ISoftwareService softwareService, UserManager<User> userManager)
    {
        _softwareService = softwareService;
        _userManager = userManager;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddSoftware([FromBody] AddSoftwareDto software)
    {
        if (!IsUserLoggedIn())
        {
            return BadRequest("User not logged in");
        }

        try
        {
            await _softwareService.AddSoftwareAsync(software);
            return Ok(software);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSoftware(int id)
    {
        if (!IsUserLoggedIn())
        {
            return BadRequest("User not logged in");
        }

        try
        {
            GetSoftwareDto software = await _softwareService.GetSoftwareAsync(id);
            return Ok(software);
        }
        catch (SoftwareNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }


    [HttpGet("list")]
    public async Task<IActionResult> GetSoftwares()
    {
        if (!IsUserLoggedIn())
        {
            return BadRequest("User not logged in");
        }

        try
        {
            List<GetSoftwareDto> softwares = await _softwareService.GetSoftwaresAsync();
            return Ok(softwares);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Software()
    {
        if (!IsUserLoggedIn())
        {
            return BadRequest("User not logged in");
        }

        List<GetSoftwareDto> softwares = await _softwareService.GetSoftwaresAsync();
        SoftwareViewModel model = new()
        {
            Softwares = softwares
        };

        return View(model);
    }


    public bool IsUserLoggedIn()
    {
        return _userManager.GetUserAsync(User).Result != null;
    }
}