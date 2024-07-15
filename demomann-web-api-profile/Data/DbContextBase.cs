namespace DemomannWebApi.Profile.Data;

internal class DbContextBase
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}