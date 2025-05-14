using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace quiz_app.Pages;

public class IndexModel : PageModel
{
    private readonly QuizContext _context;

    public IndexModel(QuizContext context)
    {
        _context = context;
    }

    public List<string> Categories { get; set; } = new List<string>();

    public async Task OnGetAsync()
    {
        Categories = await _context.Questions
            .Select(q => q.Category ?? "Uncategorized")
            .Distinct()
            .ToListAsync();
    }
}
