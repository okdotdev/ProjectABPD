using abcAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace abcAPI.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string nickname, string password)
    {
        if (true)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid login attempt.");
        return View();
    }

}

