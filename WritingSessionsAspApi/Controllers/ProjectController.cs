using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WritingSessionsAspApi.Data;
using WritingSessionsAspApi.Data.Contracts;
using WritingSessionsAspApi.Models;

namespace WritingSessionsAspApi.Controllers;
[ApiController]
[Route("projects")]
public class ProjectController : Controller
{
    private readonly IRecordRepo<Project> _projectRepo;

    public ProjectController(IRecordRepo<Project> projectRepo)
    {
        _projectRepo = projectRepo;
    }
    
    // GET: /projects
    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        List<Project> projects = await _projectRepo.GetAllRecordsAsync();
        return Ok(projects);
    }
    
    // GET: /projects/:id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        Project? project = await _projectRepo.GetRecordByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }
        return Ok(project);
    }

    [HttpGet]
    [Route("/projects/code/{code}")]
    public async Task<IActionResult> GetProjectByCode(string code)
    {
        Project? project = await _projectRepo.GetRecordByCodeAsync(code);
        if (project == null)
        {
            return NotFound();
        }
        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(Project project)
    {
        int rowsAffected = await _projectRepo.CreateRecordAsync(project);
        if (rowsAffected == 0)
        {
            return Problem("Failed to create project");
        }
        return Created();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateProject([FromRoute] int id, [FromBody] JsonPatchDocument<Project> projectData)
    {
        if (projectData == null)
        {
            return BadRequest();
        }
        Project? projectToUpdate = await _projectRepo.GetRecordByIdAsync(id);
        if (projectToUpdate == null)
        {
            return NotFound();
        }
        projectData.ApplyTo(projectToUpdate, ModelState);
        if (!TryValidateModel(projectToUpdate) || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        int rowsAffected = await _projectRepo.UpdateRecordAsync(projectToUpdate);
        if (rowsAffected == 0)
        {
            return Problem("Failed to update project");
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        Project? projectToDelete = await _projectRepo.GetRecordByIdAsync(id);
        if (projectToDelete == null)
        {
            return NotFound();
        }
        int rowsAffected = await _projectRepo.DeleteRecordAsync(projectToDelete);
        if (rowsAffected == 0)
        {
            return Problem("Failed to delete project");
        }
        return NoContent();
    }
}