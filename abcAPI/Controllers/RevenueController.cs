using abcAPI.Models.DTOs;
using abcAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace abcAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RevenueController : Controller
    {
        private readonly IRevenueService _revenueService;

        public RevenueController(IRevenueService revenueService)
        {
            _revenueService = revenueService;
        }

        [HttpPost("calculate/revenue/real")]
        public async Task<IActionResult> CalculateRealRevenue([FromBody] RevenueRequestDto requestDto)
        {
            try
            {
                RevenueResponseDto revenue = await _revenueService.CalculateRevenueAsync(requestDto);
                return Ok(revenue.CurrentRevenue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("calculate/revenue/projected")]
        public async Task<IActionResult> CalculateProjectedRevenue([FromBody] RevenueRequestDto requestDto)
        {
            try
            {
                RevenueResponseDto revenue = await _revenueService.CalculateRevenueAsync(requestDto);
                return Ok(revenue.ProjectedRevenue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult RealRevenue()
        {
            return View("Real");
        }

        public IActionResult ProjectedRevenue()
        {
            return View("Projected");
        }


    }
}