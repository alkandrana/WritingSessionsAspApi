using WritingSessionsAspApi.Models;

namespace WritingSessionsAspApi.Data;

public class SeedData
{
    public static void Seed(AppDbContext ctx)
    {
        if (!ctx.Statuses.Any())
        {
            
            Status pending = new Status
            {
                Name = "Pending"
            };
            ctx.Statuses.Add(pending);

            Status writing = new Status
            {
                Name = "Writing"
            };
            ctx.Statuses.Add(writing);

            Status finished = new Status
            {
                Name = "Finished"
            };
            ctx.Statuses.Add(finished);

            Status aborted = new Status
            {
                Name = "Aborted"
            };
            ctx.Statuses.Add(aborted);
            
            ctx.SaveChanges();
        }
    }
}