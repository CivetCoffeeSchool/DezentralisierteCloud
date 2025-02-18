using Domain.Repositories.Implementations;
using Domain.Repositories.Interfaces;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Microsoft.AspNetCore.Identity;
using Model.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddScoped<IPeerRepository, PeerRepository>();
builder.Services.AddScoped<IRepository<DataOnPeers>,ARepository<DataOnPeers>>();

builder.Services.AddDbContextFactory<NetworkinfoDbContext>(
    options => options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8,0,33))
    )
);


var app = builder.Build();

app.UseRouting();
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
