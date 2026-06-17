using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WritingSessionsAspApi.Data.Contracts;

namespace WritingSessionsAspApi.Models;

[Index(nameof(Code), nameof(ProjectId), IsUnique = true)]
public class Scene : Model
{
    [StringLength(20)]
    public string Code { get; set; } = string.Empty;
    public int Sequence { get; set; }
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
    public int Words { get; set; }
    public int StatusId { get; set; } = 3;
    public Status? Status { get; set; }
    [StringLength(255)]
    public string? Plotline { get; set; }
    public DateTime? Created { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
}