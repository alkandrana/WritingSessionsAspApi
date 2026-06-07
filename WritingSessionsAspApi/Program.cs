using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WritingSessionsAspApi.Data;
using WritingSessionsAspApi.Data.Contracts;
using WritingSessionsAspApi.Models;

namespace WritingSessionsAspApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
        builder.Services.AddDbContext<AppDbContext>(options => options
            .UseNpgsql(connectionString));
        // Configure cors to work with react frontend
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
            });
        });

        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        // ignored infinite navigation relations
        {
            options.SerializerSettings.ReferenceLoopHandling = 
                ReferenceLoopHandling.Ignore;
        });
        builder.Services.AddScoped(typeof(IRecordRepo<>), typeof(RecordRepo<>));
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddIdentityApiEndpoints<AppUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthorization();

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowSpecificOrigins");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapIdentityApi<AppUser>();

        app.Run();
    }
}