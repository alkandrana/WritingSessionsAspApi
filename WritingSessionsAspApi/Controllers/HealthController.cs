using Microsoft.AspNetCore.Mvc;

namespace WritingSessionsAspApi.Controllers;
[ApiController]
[Route("/")]
public class HealthController : Controller
{
    // GET: /
    [HttpGet]
    public IActionResult CheckHealth()
    {
        return Ok("Server is running on port 5088");
    }
}