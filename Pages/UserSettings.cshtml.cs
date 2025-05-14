using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizApp.Models;

namespace quiz_app.Pages;

public class UserSettingsModel : PageModel
{
  private readonly QuizContext _context;

  public UserSettingsModel(QuizContext context)
  {
    _context = context;
  }

  [BindProperty]
  public UserSettings Settings { get; set; } = new UserSettings();

  public void OnGet()
  {
    // Load existing settings or initialize default settings
    Settings = _context.UserSettings.FirstOrDefault() ?? new UserSettings();
  }

  public IActionResult OnPost()
  {
    if (!ModelState.IsValid)
    {
      return Page();
    }

    var existingSettings = _context.UserSettings.FirstOrDefault();
    if (existingSettings != null)
    {
      existingSettings.EnableTimer = Settings.EnableTimer;
      existingSettings.ShowFeedback = Settings.ShowFeedback;
      existingSettings.TimerDuration = Settings.TimerDuration;
    }
    else
    {
      _context.UserSettings.Add(Settings);
    }

    _context.SaveChanges();
    return RedirectToPage("/Index");
  }
}
