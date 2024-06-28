using abcAPI.Exceptions;
using abcAPI.Models.DTOs;
using abcAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace abcAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
         //   try
          //  {
                decimal revenue = await _revenueService.CalculateRealRevenueAsync(requestDto);
                return Ok(revenue);
          /*  }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } */
        }

        [HttpPost("calculate/revenue/projected")]
        public async Task<IActionResult> CalculateProjectedRevenue([FromBody] RevenueRequestDto requestDto)
        {
          //  try
          //  {
                decimal revenue = await _revenueService.CalculateProjectedRevenueAsync(requestDto);
                return Ok(revenue);
          /*  }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } */
        }

        [HttpGet("view/real")]
        public IActionResult RealRevenue()
        {
            return View("Real");
        }

        [HttpGet("view/projected")]
        public IActionResult ProjectedRevenue()
        {
            return View("Projected");
        }


    }
}