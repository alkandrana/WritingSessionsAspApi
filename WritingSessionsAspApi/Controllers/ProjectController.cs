using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WritingSessionsAspApi.Data.Contracts;
using WritingSessionsAspApi.Models;

namespace WritingSessionsAspApi.Controllers;
[ApiController]
[Route("projects")]
[Authorize]
public class ProjectController : Controller
{
    private readonly IRecordRepo<Project> _projectRepo;
    private readonly IRecordRepo<Scene> _sceneRepo;
    private readonly IRecordRepo<Status> _statusRepo;
    private readonly UserManager<AppUser> _userManager;

    public ProjectController(IRecordRepo<Project> projectRepo, UserManager<AppUser> userManager, IRecordRepo<Scene> sceneRepo, IRecordRepo<Status> statusRepo)
    {
        _projectRepo = projectRepo;
        _userManager = userManager;
        _sceneRepo = sceneRepo;
        _statusRepo = statusRepo;
    }
    
    // GET: /projects
    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        AppUser? currentUser = await _userManager.GetUserAsync(User);
        Console.WriteLine(currentUser);
        if (currentUser == null)
        {
            return Unauthorized();
        }
        List<Project> projects = await _projectRepo.GetSelectRecordsAsync(p => p.AuthorId == currentUser.Id);
        return Ok(projects);
    }

    [HttpGet]
    [Route("/projects/plotter/{projectCode}")]
    public async Task<IActionResult> GetPlotter(string projectCode)
    {
        List<Project> projects = await _projectRepo.GetRecordByCodeAsync(
            projectCode, "Code", p => p.Scenes);
        if (projects.Count == 0)
        {
            return NotFound();
        }
        Project project = projects[0];
        
        // TODO: implement a more secure way of checking for an aborted status
        List<Scene> scenes = project.Scenes.Where(s => s.StatusId != 6).ToList();
        scenes.Sort((x, y) => x.Sequence.CompareTo(y.Sequence));
        List<PlotVM> plotter = new List<PlotVM>();
        int wordCount = 0;
        foreach (Scene sc in scenes)
        {
            wordCount += sc.Words;
            string? status = _statusRepo.GetRecordByIdAsync(sc.StatusId).Result?.Name;
            PlotVM entry = new PlotVM
            {
                Sequence = sc.Sequence,
                Name = sc.Name,
                WordCount = sc.Words,
                TSF = wordCount,
                POT = (wordCount / (float)project.Goal) * 100,
                Plotline = sc.Plotline,
                Status = status
            };
            entry.SetBeat();
            plotter.Add(entry);
        }
        return Ok(plotter);
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
        List<Project> projects = await _projectRepo.GetRecordByCodeAsync(code, "Code");
        if (projects.Count == 0)
        {
            return NotFound();
        }
        return Ok(projects);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(Project project)
    {
        AppUser? currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return Unauthorized();
        }
        project.AuthorId = currentUser.Id;
        if (project.Created == null)
        {
            project.Created = DateTime.UtcNow;
        }
        List<Project> duplicates = await _projectRepo.GetRecordByCodeAsync(project.Code, "Code");
        if (duplicates.Count > 0)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Duplicate code",
                Detail = "A project with the same code already exists.",
            });
        }
        int rowsAffected = await _projectRepo.CreateRecordAsync(project);
        if (rowsAffected == 0)
        {
            return Problem("Failed to create project");
        }
        return Created();
    }

    [HttpPatch("{id:int}")]
    // request format: { "op": "replace", "path": "/title", "value": "The Shadow of Death" }
    // Path refers to the property you're targeting
    // If changing more than one property, send array of objects
    // Even if only changing one property, must be formatted as an array
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