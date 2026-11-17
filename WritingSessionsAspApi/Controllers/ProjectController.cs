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

    public ProjectController(
        IRecordRepo<Project> projectRepo,
        UserManager<AppUser> userManager,
        IRecordRepo<Scene> sceneRepo,
        IRecordRepo<Status> statusRepo
    )
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
        List<Project> projects = _projectRepo
            .GetSelectRecordsAsync(p => p.AuthorId == currentUser.Id)
            .Result.OrderBy(p => p.Series)
            .ThenBy(p => p.Volume)
            .ToList();
        return Ok(projects);
    }

    [HttpGet]
    [Route("/projects/plotter/{projectCode}")]
    public async Task<IActionResult> GetPlotter(string projectCode)
    {
        Project? project = await _projectRepo.GetRecordByUniquePropAsync(
            projectCode,
            "Code",
            p => p.Scenes
        );
        if (project == null)
        {
            return NotFound();
        }
        Status? abortStatus = await _statusRepo.GetRecordByUniquePropAsync("Aborted", "Name");
        int abortId;
        if (abortStatus != null)
        {
            abortId = abortStatus.Id;
        }
        else
        {
            abortId = 0;
        }
        // TODO: implement a more secure way of checking for an aborted status
        List<Scene> scenes = project.Scenes.Where(s => s.StatusId != abortId).ToList();
        scenes.Sort((x, y) => x.Sequence.CompareTo(y.Sequence));
        List<PlotVM> plotter = new List<PlotVM>();
        int wordCount = 0;
        foreach (Scene sc in scenes)
        {
            wordCount += sc.Words;
            string? status = _statusRepo.GetRecordByIdAsync(sc.StatusId).Result?.Name;
            if (sc.Plotline == null)
            {
                sc.Plotline = "";
            }
            PlotVM entry = new PlotVM
            {
                Sequence = sc.Sequence,
                Name = sc.Name,
                WordCount = sc.Words,
                TSF = wordCount,
                POT = (wordCount / (float)project.Goal) * 100,
                Plotline = sc.Plotline,
                Status = status,
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
        Project? project = await _projectRepo.GetRecordByUniquePropAsync(code, "Code");
        if (project == null)
        {
            return NotFound();
        }
        return Ok(project);
    }

    [HttpGet]
    [Route("/projects/name/{title}")]
    public async Task<IActionResult> GetProjectByTitle(string title)
    {
        List<Project> projects = await _projectRepo.GetSelectRecordsAsync(p =>
            p.Title.ToLower().Contains(title.ToLower())
        );
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
        Project? duplicate = await _projectRepo.GetRecordByUniquePropAsync(project.Code, "Code");
        if (duplicate != null)
        {
            return Conflict(
                new ProblemDetails
                {
                    Title = "Duplicate code",
                    Detail = "A project with the same code already exists.",
                }
            );
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
    public async Task<IActionResult> UpdateProject(
        [FromRoute] int id,
        [FromBody] JsonPatchDocument<Project> projectData
    )
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
