using Microsoft.EntityFrameworkCore;
using NewsParser.API;
using NewsParser.API.StartupExtensions;
using NewsParser.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices();
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
var app = builder.Build();

string? newsUrl = builder.Configuration["NewsSourceURL:Habr"];
if (newsUrl != null)
    await DbInitializer.InitDb(app.Services, newsUrl);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
