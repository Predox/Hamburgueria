using Hamburgueria.Infrastructure.Data;
using MediatR;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Hamburgueria.Application.Queries.CalcularFrete;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddSingleton<InMemoryDatabase>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(InMemoryDatabase).Assembly);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(
  typeof(CalcularFreteHandler).Assembly
);

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
