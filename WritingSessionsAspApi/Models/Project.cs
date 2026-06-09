using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WritingSessionsAspApi.Data.Contracts;

namespace WritingSessionsAspApi.Models;

[Index(nameof(Code), nameof(AuthorId), IsUnique = true)]
public class Project : Model
{
    [StringLength(5)] public string Code { get; set; } = string.Empty;
    [StringLength(255)] public string Title { get; set; } = string.Empty;
    [StringLength(255)] public string Series { get; set; } = string.Empty;
    public int Goal { get; set; }
    public string AuthorId { get; set; }
    [ForeignKey("AuthorId")] public AppUser? Author { get; set; }
}