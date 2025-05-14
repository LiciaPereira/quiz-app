using Microsoft.EntityFrameworkCore;

namespace QuizApp.Models
{
  public class QuizContext : DbContext
  {
    public QuizContext(DbContextOptions<QuizContext> options) : base(options) { }

    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    //seed data for testing
    public static void SeedData(QuizContext context)
    {
      if (!context.Questions.Any())
      {
        var sampleQuestions = new List<Question>
            {
                new Question
                {
                    Text = "What is the capital of France?",
                    Category = "Geography",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "Paris", IsCorrect = true },
                        new Answer { Text = "London", IsCorrect = false },
                        new Answer { Text = "Berlin", IsCorrect = false }
                    }
                },
                new Question
                {
                    Text = "What is 2 + 2?",
                    Category = "Math",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "3", IsCorrect = false },
                        new Answer { Text = "4", IsCorrect = true },
                        new Answer { Text = "5", IsCorrect = false }
                    }
                }
            };

        context.Questions.AddRange(sampleQuestions);
        context.SaveChanges();
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
