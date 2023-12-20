using NewsParser.API;
using NewsParser.API.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices();

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
