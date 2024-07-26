namespace DemomannWebApi.Profile.Services.Models;

public class Profile 
{
    public string? FirstName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public string? Email {get; set;} = null;
public string? EmailReenter { get; set; } = null;
    public string? Phone { get; set; }
    public string? Password { get; set; } = null;
    public string? PasswordReEnter { get; set; } = null;
}