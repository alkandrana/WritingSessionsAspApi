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

    [HttpGet]
    [Route("/status/name/{name}")]
    public async Task<IActionResult> GetOptionByName(string name)
    {
        List<Status> options = await _statusRepo.GetRecordByCodeAsync(name, "Name");
        return Ok(options);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStatusById(int id)
    {
        Status? status = await _statusRepo.GetRecordByIdAsync(id);
        if (status == null)
        {
            return NotFound();
        }

        return Ok(status);
    }
}
