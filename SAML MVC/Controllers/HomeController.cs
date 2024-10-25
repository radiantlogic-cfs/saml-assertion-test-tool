using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SAML_MVC.Controllers;

public class HomeController : Controller
{
    public Task<IActionResult> Index()
    {
        return Task.FromResult<IActionResult>(View());
    }

    [Authorize]
    public IActionResult Dashboard()
    {
        return Accepted();
    }
}