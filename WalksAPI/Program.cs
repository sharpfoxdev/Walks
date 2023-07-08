using Microsoft.EntityFrameworkCore;
using WalksAPI.Data;
using WalksAPI.Mappings;
using WalksAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dependency injection
builder.Services.AddDbContext<WalksDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WalksConnectionString")));

//injects IRegionRepository with the implementation SQLRegionRepository
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
var app = builder.Build();

// migrations - creates tables in the database
// tools > nuget package manager > package manager console
// Add-Migration "nameOfMigration" (for example "Initial Migration")
// Update-Database

// Configure the HTTP request pipeline.
// this is middleware, it handles requests and responses
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
