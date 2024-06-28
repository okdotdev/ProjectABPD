using abcAPI.Exceptions;
using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;
using abcAPI.Models.ViewModels;
using abcAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace abcAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    public async Task<IActionResult> AddSoftware([FromForm] AddSoftwareDto software)
    {
        try
        {
            await _softwareService.AddSoftwareAsync(software);
            return RedirectToAction("Software");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSoftware(int id)
    {
        try
        {
            GetSoftwareDto software = await _softwareService.GetSoftwareAsync(id);
            return Ok(software);
        }
        catch (NotFoundException e)
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

    [HttpGet("view")]
    public async Task<IActionResult> Software()
    {
        List<GetSoftwareDto> softwares = await _softwareService.GetSoftwaresAsync();
        SoftwareViewModel model = new()
        {
            Softwares = softwares
        };

        return View(model);
    }
}