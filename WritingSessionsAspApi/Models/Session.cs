using Microsoft.EntityFrameworkCore;
using WritingSessionsAspApi.Data.Contracts;

namespace WritingSessionsAspApi.Models;

[Index(nameof(StartTime), nameof(StopTime), IsUnique = true)]
public class Session : Model
{
    public DateOnly Date { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? StopTime { get; set; }
    public int Words { get; set; }
    public int SceneId { get; set; }
    public Scene? Scene { get; set; }
    
    public AppUser? Author { get; set; }
    public string? Comments { get; set; }

    public double? Duration
    {
        get
        {
            if (StartTime != null && StopTime != null)
            {
                TimeSpan? duration = StopTime - StartTime;
                double? minutes = duration?.TotalMinutes;
                return minutes;
            }
            return null;
        }
    }

    public double? Wpm
    {
        get
        {
            if (Duration != null)
            {
                double? wpm = Words / Duration;
                return wpm;
            }
            return null;
        }
    }
}