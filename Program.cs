using BibliotecaApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var arquivo = Path.Combine(builder.Environment.ContentRootPath, "biblioteca.db");
builder.Services.AddDbContext<BibliotecaContext>(op => op.UseSqlite($"Data Source={arquivo}"));
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BibliotecaContext>();
    db.Database.EnsureCreated();
}
app.MapControllers();
app.Run();
