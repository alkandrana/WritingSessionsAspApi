using System.Reflection;

namespace WritingSessionsAspApi.Data.Contracts;

public abstract class Model
{
    public int Id { get; set; }

    public void ApplyTo(Model model)
    {
        PropertyInfo[] properties = model.GetType().GetProperties();
        foreach (PropertyInfo prop in properties)
        {
            if (!prop.Name.ToLower().Contains("id"))
            {
                prop.SetValue(model, prop.GetValue(this));
            }
        }
    }
}