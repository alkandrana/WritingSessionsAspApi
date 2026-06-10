using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WritingSessionsAspApi.Data;
using WritingSessionsAspApi.Data.Contracts;
using WritingSessionsAspApi.Models;

namespace WritingSessionsAspApi.Controllers;
[ApiController]
[Route("scenes")]
public class SceneController : Controller
{
    private readonly IRecordRepo<Scene> _sceneRepo;
    private readonly AppDbContext _ctx;

    public SceneController(IRecordRepo<Scene> sceneRepo, AppDbContext ctx)
    {
        _sceneRepo = sceneRepo;
        _ctx = ctx;
    }
    
    // GET: /sessions
    [HttpGet]
    public async Task<IActionResult> GetAllScenes()
    {
        List<Scene> scenes = await _sceneRepo.GetAllRecordsAsync();
        return Ok(scenes);
    }
    
    // GET: /sessions/:id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSceneById(int id)
    {
        Scene? scene = await _sceneRepo.GetRecordByIdAsync(id);
        if (scene == null)
        {
            return NotFound();
        }
        return Ok(scene);
    }

    [HttpGet]
    [Route("/scenes/code/{code}")]
    public async Task<IActionResult> GetSceneByCode(string code)
    {
        Scene? scene = await _sceneRepo.GetRecordByCodeAsync(code, "Code", sc => sc.Project);
        if (scene == null)
        {
            return NotFound();
        }
        return Ok(scene);
    }

    [HttpGet]
    [Route("/scenes/project/{projectId:int}")]
    public async Task<IActionResult> GetScenesByProject(int projectId)
    {
        List<Scene> scenes = await _sceneRepo.GetSelectRecordsAsync(sc => sc.ProjectId == projectId);
        return Ok(scenes);
    }

    [HttpPost]
    public async Task<IActionResult> CreateScene(Scene scene)
    {
        int rowsAffected = await _sceneRepo.CreateRecordAsync(scene);
        if (rowsAffected == 0)
        {
            return Problem("Failed to create scene");
        }
        return Created();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateScene([FromRoute] int id, [FromBody] JsonPatchDocument<Scene> sceneData)
    {
        if (sceneData == null)
        {
            return BadRequest();
        }
        Scene? sceneToUpdate = await _sceneRepo.GetRecordByIdAsync(id);
        if (sceneToUpdate == null)
        {
            return NotFound();
        }
        sceneData.ApplyTo(sceneToUpdate, ModelState);
        if (!TryValidateModel(sceneToUpdate) || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        int rowsAffected = await _sceneRepo.UpdateRecordAsync(sceneToUpdate);
        if (rowsAffected == 0)
        {
            return Problem("Failed to update scene");
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteScene(int id)
    {
        Scene? sceneToDelete = await _sceneRepo.GetRecordByIdAsync(id);
        if (sceneToDelete == null)
        {
            return NotFound();
        }
        int rowsAffected = await _sceneRepo.DeleteRecordAsync(sceneToDelete);
        if (rowsAffected == 0)
        {
            return Problem("Failed to delete scene");
        }
        return NoContent();
    }
}