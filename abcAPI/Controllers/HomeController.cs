using abcAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace abcAPI.Controllers;


public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }




}