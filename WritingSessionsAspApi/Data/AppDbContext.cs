using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WritingSessionsAspApi.Models;

namespace WritingSessionsAspApi.Data;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Scene> Scenes => Set<Scene>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Status> Statuses => Set<Status>();
}