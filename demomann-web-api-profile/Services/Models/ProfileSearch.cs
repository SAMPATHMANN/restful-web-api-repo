using Demomann.common.Models;

namespace DemomannWebApi.Profile.Services.Models;

public class ProfileSearch : Pagination<Profile>
{
    public string? Email { get; set; } = null;

    public string? Password {get; set;} = null;

    public bool IsValidUser {get; set;} = false;

    public bool Authenticate { get; set; }

}