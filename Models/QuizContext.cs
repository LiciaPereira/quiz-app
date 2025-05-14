using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace QuizApp.Models
{
  public class QuizContext : DbContext
  {
    public QuizContext(DbContextOptions<QuizContext> options) : base(options) { }

    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }

    public static void SeedDataFromJson(QuizContext context, string jsonFilePath)
    {
      if (!File.Exists(jsonFilePath))
      {
        throw new FileNotFoundException($"JSON file not found: {jsonFilePath}");
      }

      var jsonData = File.ReadAllText(jsonFilePath);
      Console.WriteLine("Seeding data from JSON file: " + jsonFilePath);
      Console.WriteLine("JSON Data: " + jsonData);

      var questions = JsonSerializer.Deserialize<List<Question>>(jsonData, new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      });

      if (questions != null && !context.Questions.Any())
      {
        Console.WriteLine("Seeding the following categories:");
        foreach (var question in questions)
        {
          Console.WriteLine("- " + question.Category);
        }

        context.Questions.AddRange(questions);
        context.SaveChanges();
        Console.WriteLine("Seeding completed.");
      }
      else
      {
        Console.WriteLine("No data to seed or data already exists.");
      }
    }
  }

  public class Question
  {
    public int Id { get; set; }
    public string? Text { get; set; } //nullable to avoid initialization issues
    public string? Category { get; set; } //category for different quizzes
    public ICollection<Answer>? Answers { get; set; } //nullable to avoid initialization issues
  }

  public class Answer
  {
    public int Id { get; set; }
    public string? Text { get; set; } //nullable to avoid initialization issues
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
    public Question? Question { get; set; } //nullable to avoid initialization issues
  }
}
