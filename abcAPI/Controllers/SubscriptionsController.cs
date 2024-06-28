using abcAPI.Exceptions;
using abcAPI.Models.DTOs;
using abcAPI.Models.ViewModels;
using abcAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace abcAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionsController : Controller
{
    private ISubscriptionService _subscriptionService;

    public SubscriptionsController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }


    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromForm] SubscribeDto subscribeDto)
    {
      //  try
      //  {
            await _subscriptionService.SubscribeAsync(subscribeDto);
            return RedirectToAction("Subscriptions");
     //   }
      //  catch (NotFoundException ex)
       // {
          //  return NotFound(ex.Message);
     //   }
      //  catch (ArgumentException ex)
      //  {
      //      return BadRequest(ex.Message);
     //   }
      //  catch (Exception ex)
      //  {
      //      return StatusCode(500, ex.Message);
      //  }
    }


    [HttpGet("list")]
    public async Task<IActionResult> GetSubscriptionsList()
    {
        try
        {
            List<GetSubscriptionDto> subscriptions = await _subscriptionService.GetSubscriptionsListAsync();
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

    [HttpPost("pay")]
    public async Task<IActionResult> PayForSubscription([FromForm] PaymentDto payForSubscriptionDto)
    {
        try
        {
            await _subscriptionService.PayForSubscriptionAsync(payForSubscriptionDto);
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

    [HttpGet("subscriptions")]
    public async Task<IActionResult> Subscriptions()
    {
        List<GetSubscriptionDto> subscriptions = await _subscriptionService.GetSubscriptionsListAsync();
        SubscriptionViewModel model = new()
        {
            Subscriptions = subscriptions
        };

        return View(model);
    }



}