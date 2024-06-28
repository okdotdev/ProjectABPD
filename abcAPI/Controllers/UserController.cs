using abcAPI.Models;
using abcAPI.Models.TableModels;
using abcAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace abcAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class UserController : Controller
{
    private readonly SignInManager<User> _signInManager;


    public UserController( SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }



    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromForm] LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        SignInResult result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
        if (result.Succeeded)
        {
            return RedirectToAction("ControlPanel", "User");

        }

        Console.Error.WriteLine("Login failed");

        ModelState.AddModelError("", "Invalid login attempt.");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "User");
    }

    public IActionResult ControlPanel()
    {
        return View();
    }
}