namespace DemomannWebApi.Profile.Services.Models;

public class Profile {
    public string? Id { get; set; }
    public string? Name { get; set; } = null;
    public string? Email {get; set;} = null;
    public string? Password { get; set; } = null;
}