using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SAML_MVC.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Dashboard()
    {
        return View();
    }
}