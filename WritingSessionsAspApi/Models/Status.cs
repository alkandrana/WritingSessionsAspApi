namespace WritingSessionsAspApi.Models;

public class Status
{
    public int Id { get; set; }
    // Can be one of: 'pending','writing','finished','aborted'
    public string Name { get; set; }
}