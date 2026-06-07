using Microsoft.AspNetCore.Mvc;

namespace WritingSessionsAspApi.Controllers;
[ApiController]
[Route("sessions")]
public class SessionController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}