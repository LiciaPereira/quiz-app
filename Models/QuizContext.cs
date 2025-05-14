using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace QuizApp.Models
{
  public class QuizContext : DbContext
  {
    public QuizContext(DbContextOptions<QuizContext> options) : base(options) { }

    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }

    public static void SeedDataFromJson(QuizContext context, string jsonFilePath)
    {
      if (!File.Exists(jsonFilePath))
      {
        Console.WriteLine($"Error: JSON file not found: {jsonFilePath}");
        return;
      }

      try
      {
        var jsonData = File.ReadAllText(jsonFilePath);
        var questions = JsonSerializer.Deserialize<List<Question>>(jsonData, new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        });

        if (questions == null || !questions.Any())
        {
          Console.WriteLine("Error: No questions found in the JSON file.");
          return;
        }

        if (context.Questions.Any())
        {
          Console.WriteLine("Seeding skipped: Questions already exist.");
          return;
        }

        context.Questions.AddRange(questions);
        context.SaveChanges();
        Console.WriteLine("Seeding completed successfully.");
      }
      catch (JsonException ex)
      {
        Console.WriteLine("JSON parsing error: " + ex.Message);
      }
      catch (DbUpdateException ex)
      {
        Console.WriteLine("Database update error: " + ex.Message);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Unexpected error: " + ex.Message);
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

  public class UserSettings
  {
    public int Id { get; set; }
    public bool EnableTimer { get; set; } = false;
    public bool ShowFeedback { get; set; } = false;
    public int TimerDuration { get; set; } = 60; // Default to 60 seconds
  }
}
