using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Logging;

public class HomeController : Controller
{
    private readonly NoteContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(NoteContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index(string query)
    {
        query = query?.ToLower();

        var notes = string.IsNullOrWhiteSpace(query)
          ? _context.Notes.ToList()
          : _context.Notes
            .Where(n => n.Title.ToLower().Contains(query))
            .ToList();

        return View(notes);
    }

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(Note note)
    {
        if (ModelState.IsValid)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(note);
    }

    public IActionResult Edit(int id)
    {
        var note = _context.Notes.Find(id);
        if (note == null)
        {
            return NotFound();
        }

        _logger.LogInformation("Retrieved note: {Note}", note);
        _logger.LogInformation("Note Id: {Id}, Title: {Title}, Content: {Content}", note.Id, note.Title, note.Content);

        return View(note);
    }




    [HttpPost]
    public IActionResult Edit([Bind("Id,Title,Content")] Note note)
    {
        if (ModelState.IsValid)
        {
            var noteFromDb = _context.Notes.Find(note.Id);

            noteFromDb.Title = note.Title;
            noteFromDb.Content = note.Content;

            _context.Notes.Update(noteFromDb);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(note);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var note = _context.Notes.Find(id);
        if (note == null)
        {
            return NotFound();
        }
        _context.Notes.Remove(note);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}