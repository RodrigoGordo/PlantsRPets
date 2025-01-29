using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
//using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PlantsRPetsProjetoServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlantsRPetsProjetoServerContext") ?? throw new InvalidOperationException("Connection string 'PlantsRPetsProjetoServerContext' not found.")));
//builder.Services.AddDbContext<PlantsRPetsProjetoServerContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("PlantsRPetsProjetoServerContext") ?? throw new InvalidOperationException("Connection string 'PlantsRPetsProjetoServerContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddDbContext<PlantsRPetsProjetoServerContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("PlantsRPetsProjetoServerContext")));








// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
