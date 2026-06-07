using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WritingSessionsAspApi.Data;
using WritingSessionsAspApi.Data.Contracts;
using WritingSessionsAspApi.Models;

namespace WritingSessionsAspApi.Controllers;
[ApiController]
[Route("sessions")]
public class SessionController : Controller
{
    private readonly IRecordRepo<Session> _sessionRepo;
    private readonly AppDbContext _ctx;

    public SessionController(IRecordRepo<Session> sessionRepo, AppDbContext ctx)
    {
        _sessionRepo = sessionRepo;
        _ctx = ctx;
    }
    
    // GET: /sessions
    [HttpGet]
    public async Task<IActionResult> GetAllSessions()
    {
        List<Session> sessions = await _sessionRepo.GetAllRecordsAsync();
        return Ok(sessions);
    }
    
    // GET: /sessions/:id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOneSession(int id)
    {
        Session? session = await _sessionRepo.GetRecordByIdAsync(id);
        if (session == null)
        {
            return NotFound();
        }
        return Ok(session);
    }

    [HttpGet]
    [Route("/sessions/scene/{sceneId:int}")]
    public async Task<IActionResult> GetSessionsByScene(int sceneId)
    {
        List<Session> sessions = await _ctx.Sessions.Where(ses => ses.SceneId == sceneId).ToListAsync();
        return Ok(sessions);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSession(Session session)
    {
        int rowsAffected = await _sessionRepo.CreateRecordAsync(session);
        if (rowsAffected == 0)
        {
            return Problem("Failed to create session");
        }
        return Created();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateSession([FromRoute] int id, [FromBody] JsonPatchDocument<Session> sessionData)
    {
        if (sessionData == null)
        {
            return BadRequest();
        }
        Session? sessionToUpdate = await _sessionRepo.GetRecordByIdAsync(id);
        if (sessionToUpdate == null)
        {
            return NotFound();
        }
        sessionData.ApplyTo(sessionToUpdate, ModelState);
        if (!TryValidateModel(sessionToUpdate) || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        int rowsAffected = await _sessionRepo.UpdateRecordAsync(sessionToUpdate);
        if (rowsAffected == 0)
        {
            return Problem("Failed to update session");
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSession(int id)
    {
        Session? sessionToDelete = await _sessionRepo.GetRecordByIdAsync(id);
        if (sessionToDelete == null)
        {
            return NotFound();
        }
        int rowsAffected = await _sessionRepo.DeleteRecordAsync(sessionToDelete);
        if (rowsAffected == 0)
        {
            return Problem("Failed to delete session");
        }
        return NoContent();
    }
        
    
}