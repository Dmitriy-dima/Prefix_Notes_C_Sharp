using Microsoft.EntityFrameworkCore;

public class NoteContext : DbContext
{
    public DbSet<Note> Notes { get; set; }

    public NoteContext(DbContextOptions<NoteContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Only configure the database if it hasn't been configured yet.
        if (!options.IsConfigured)
        {
            options.UseSqlite("Data Source=notes.db");
        }
    }
}
