using System.Text.Json.Serialization;
using BibliotecaApi.Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//area de servicios

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler= ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));
var app = builder.Build();

//area de middlewares

app.MapControllers();
app.Run();
