using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace quiz_app.Pages;

public class QuizModel : PageModel
{
  private readonly QuizContext _context;

  public QuizModel(QuizContext context)
  {
    _context = context;
  }

  [BindProperty(SupportsGet = true)]
  public string Category { get; set; } = string.Empty;

  public List<Question> Questions { get; set; } = new List<Question>();

  public async Task OnGetAsync()
  {
    Questions = await _context.Questions
        .Include(q => q.Answers)
        .Where(q => q.Category == Category)
        .ToListAsync();
  }

  public async Task<IActionResult> OnPostAsync()
  {
    //handle form submission and calculate results here
    await Task.CompletedTask; // placeholder to resolve async warning
    return RedirectToPage("/Index");
  }
}
