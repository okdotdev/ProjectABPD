using abcAPI.Exceptions;
using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;
using abcAPI.Models.ViewModels;
using abcAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace abcAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContractController : Controller
{
    private readonly IContractService _contractService;
    private readonly UserManager<User> _userManager;

    public ContractController(IContractService contractService, UserManager<User> userManager)
    {
        _contractService = contractService;
        _userManager = userManager;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateContract([FromBody] CreateContractDto createContractDto)
    {
        try
        {
            await _contractService.CreateContractAsync(createContractDto);
            return RedirectToAction("Contracts");
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("pay")]
    public async Task<IActionResult> PayForContract([FromForm] PaymentDto paymentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _contractService.PayForContractAsync(paymentDto);
            return RedirectToAction("Contracts");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("payment")]
    public async Task<IActionResult> Payment(int contractId)
    {
        Contract? contract = await _contractService.GetContractByIdAsync(contractId);
        if (contract == null)
        {
            return NotFound("Contract not found");
        }

        PaymentViewModel model = new PaymentViewModel
        {
            ContractId = contractId,
            LeftAmount = contract.Price - contract.AmountPaid
        };

        return View(model);
    }

    [HttpPost("sign")]
    public async Task<IActionResult> SignContract([FromForm] int contractId)
    {
        try
        {
            await _contractService.SignContractAsync(contractId);
            return RedirectToAction("Contracts");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //ponownie używam posta zamiast delte ponieważ nie jest obsługiwany przez form
    [HttpPost("delete")]
    public async Task<IActionResult> DeleteContract([FromForm] int contractId)
    {
        try
        {
            await _contractService.DeleteContractAsync(contractId);
            return RedirectToAction("Contracts");
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("contracts")]
    public async Task<IActionResult> GetContracts()
    {
        try
        {
            List<GetContractDto> contracts = await _contractService.GetContractsAsync();
            return Ok(contracts);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Contracts()
    {
        List<GetContractDto> contracts = await _contractService.GetContractsAsync();
        ContractViewModel model = new()
        {
            Contracts = contracts
        };

        return View(model);
    }
}