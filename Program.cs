using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

var builder = WebApplication.CreateBuilder(args);

//add services to the container
builder.Services.AddRazorPages();

//configure SQLite database
builder.Services.AddDbContext<QuizContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("QuizDatabase")));

//remember to configure the app for deployment in the future
var app = builder.Build();

//seed database with data from JSON file
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<QuizContext>();
    var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "quiz_questions.json");
    QuizContext.SeedDataFromJson(context, jsonFilePath);
}

//configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
