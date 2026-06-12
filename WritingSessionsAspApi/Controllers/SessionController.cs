using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WritingSessionsAspApi.Data.Contracts;
using WritingSessionsAspApi.Models;

namespace WritingSessionsAspApi.Controllers;
[ApiController]
[Route("sessions")]
[Authorize]
public class SessionController : Controller
{
    private readonly IRecordRepo<Session> _sessionRepo;
    private readonly UserManager<AppUser> _userManager;
    public SessionController(IRecordRepo<Session> sessionRepo, UserManager<AppUser> userManager)
    {
        _sessionRepo = sessionRepo;
        _userManager = userManager;
    }
    
    // GET: /sessions
    [HttpGet]
    public async Task<IActionResult> GetAllSessions()
    {
        AppUser? currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return Unauthorized();
        }
        List<Session> sessions = await _sessionRepo
            .GetSelectRecordsAsync(
                s => s.Author.Id == currentUser.Id,
                s => s.Scene
                );
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
        List<Session> sessions = await _sessionRepo.GetSelectRecordsAsync(
            ses => ses.SceneId == sceneId, s => s.Scene);
        return Ok(sessions);
    }

    [HttpGet]
    [Route("/sessions/project/{projectId:int}")]
    public async Task<IActionResult> GetSessionsByProject(int projectId)
    {
        List<Session> sessions = await _sessionRepo.GetSelectRecordsAsync(
            ses => ses.Scene.ProjectId == projectId,
            s => s.Scene);
        return Ok(sessions);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSession(Session session)
    {
        AppUser? currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return Unauthorized();
        }
        session.Author = currentUser;
        List<Session> duplicates = await _sessionRepo.GetSelectRecordsAsync(
            ses => ses.StartTime == session.StartTime && ses.StopTime == session.StopTime);
        if (duplicates.Any())
        {
            return Conflict(new ProblemDetails
            {
                Title = "Duplicate session timeframe",
                Detail = "A session with those times already exists.",
            });
        }
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