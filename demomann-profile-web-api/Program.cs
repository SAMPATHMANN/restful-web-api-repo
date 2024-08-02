using DemomannWebApi.Profile.Data;
using DemomannWebApi.Profile.Data.Extensions;
using DemomannWebApi.Profile.Repos;
using DemomannWebApi.Profile.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DemomannDbContext>(
    builder.Configuration.GetSection("DemomannMongoDb"));

RegisterMongoDbEntities.Register();

builder.Services.AddControllers();

//Register Repositories
builder.Services.AddScoped<IProfileRepo, ProfileRepo>();

//Register Services
builder.Services.AddScoped<IProfileService, ProfileService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());


app.Run();
