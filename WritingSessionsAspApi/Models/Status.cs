using Microsoft.EntityFrameworkCore;
using WritingSessionsAspApi.Data.Contracts;

namespace WritingSessionsAspApi.Models;

[Index(nameof(Name), IsUnique = true)]
public class Status : Model
{
    // Can be one of: 'pending','writing','finished','aborted'
    public string Name { get; set; } = "";
}
