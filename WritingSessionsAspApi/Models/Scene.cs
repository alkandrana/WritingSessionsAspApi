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
    public Status Status { get; set; } = new Status { Name = "pending" };
    [StringLength(255)]
    public string? Plotline { get; set; }
    public int ProjectId { get; set; }
}