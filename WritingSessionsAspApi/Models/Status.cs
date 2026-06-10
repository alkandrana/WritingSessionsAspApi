using WritingSessionsAspApi.Data.Contracts;

namespace WritingSessionsAspApi.Models;

public class Status : Model
{
    // Can be one of: 'pending','writing','finished','aborted'
    public string Name { get; set; }
}