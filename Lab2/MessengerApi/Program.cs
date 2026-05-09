using System.Data;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Реєструємо підключення до PostgreSQL
builder.Services.AddTransient<IDbConnection>((sp) =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }
