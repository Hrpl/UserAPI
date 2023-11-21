using Microsoft.EntityFrameworkCore;
using NewApplicationAPI.Models;


var builder = WebApplication.CreateBuilder();
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

// получение данных
app.MapGet("/1", (ApplicationContext db) => db.Users.ToList());

app.Run();