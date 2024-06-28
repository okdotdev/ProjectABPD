using abcAPI.Models.DTOs;
using abcAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace abcAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RaportController : Controller
{
    private readonly IRaportService _raportService;
    private readonly IContractService _contractService;

    public RaportController(IRaportService raportService, IContractService contractService)
    {
        _raportService = raportService;
        _contractService = contractService;
    }

    [HttpGet("contracts/pdf")]
    public async Task<IActionResult> GetContractsPdf()
    {
        List<GetContractDto> contracts = await _contractService.GetContractsAsync();
        byte[] pdf = await _raportService.GeneratePdfReportAsync(contracts);
        return File(pdf, "application/pdf", "ContractsReport.pdf");
    }
}