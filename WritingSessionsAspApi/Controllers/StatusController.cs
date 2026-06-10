using Microsoft.AspNetCore.Mvc;
using WritingSessionsAspApi.Data.Contracts;
using WritingSessionsAspApi.Models;

namespace WritingSessionsAspApi.Controllers;
[ApiController]
[Route("status")]
public class StatusController : Controller
{
    private readonly IRecordRepo<Status> _statusRepo;

    public StatusController(IRecordRepo<Status> statusRepo)
    {
        _statusRepo = statusRepo;
    }
    // GET: /
    [HttpGet]
    public async Task<IActionResult> GetStatusOptions()
    {
        List<Status> statusOptions = await _statusRepo.GetAllRecordsAsync();
        return Ok(statusOptions);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetOptionByName(string name)
    {
        Status? option = await _statusRepo.GetRecordByCodeAsync(name, "Name");
        if (option == null)
        {
            return NotFound();
        }
        return Ok(option);
    }
}