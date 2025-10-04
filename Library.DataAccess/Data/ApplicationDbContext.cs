using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Book entity
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ISBN).HasMaxLength(20);
            entity.Property(e => e.Genre).HasMaxLength(50);
        });

        // Seed data
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", ISBN = "978-0132350884", Year = 2008, Pages = 464, Genre = "Programming" },
            new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "David Thomas, Andrew Hunt", ISBN = "978-0135957059", Year = 2019, Pages = 352, Genre = "Programming" },
            new Book { Id = 3, Title = "Design Patterns", Author = "Gang of Four", ISBN = "978-0201633610", Year = 1994, Pages = 395, Genre = "Programming" }
        );
    }
}