namespace WritingSessionsAspApi.Data.Contracts;

public abstract class Model
{
    public int Id { get; set; }
    
    public abstract void ApplyTo(Model model);
}