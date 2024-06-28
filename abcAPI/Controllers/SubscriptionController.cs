using abcAPI.Exceptions;
using abcAPI.Models.DTOs;
using abcAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace abcAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionController : Controller
{
    //ISubscriptionService _subscriptionService;

    public SubscriptionController() //(ISubscriptionService subscriptionService)
    {
        //_subscriptionService = subscriptionService;
    }


    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromForm] SubscribeDto subscribeDto)
    {
        try
        {
            //await _subscriptionService.SubscribeAsync(subscribeDto);
            return RedirectToAction("Subscriptions");
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

    [HttpPost("unsubscribe")]
    public async Task<IActionResult> Unsubscribe([FromForm] SubscribeDto subscribeDto)
    {
        try
        {
            //await _subscriptionService.UnsubscribeAsync(subscribeDto);
            return RedirectToAction("Subscriptions");
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

    [HttpGet("list")]
    public async Task<IActionResult> GetSubscriptionsList()
    {
        try
        {
            //var subscriptions = await _subscriptionService.GetSubscriptionsListAsync();
            return Ok();
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

    [HttpGet("view")]
    public async Task<IActionResult> Subscriptions()
    {
        //List<GetSubscriptionDto> softwares = await _Service.GetSoftwaresAsync();
        SubscriptionViewModel model = new()
        {
        };

        return View(model);
    }
}