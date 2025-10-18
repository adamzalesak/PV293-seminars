using Library.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.EntityConfigurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.ISBN)
            .HasMaxLength(20);

        builder.Property(e => e.Genre)
            .HasMaxLength(50);

        builder.Property(e => e.IsAvailable)
            .IsRequired()
            .HasDefaultValue(true);

        // Configure relationship with Author
        builder.HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<Book> builder)
    {
        var authorId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var authorId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var authorId3 = Guid.Parse("33333333-3333-3333-3333-333333333333");

        var bookId1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var bookId2 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var bookId3 = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

        // Using anonymous types for seeding entities with private setters
        var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new
            {
                Id = bookId1,
                Title = "Clean Code",
                AuthorId = authorId1,
                ISBN = "978-0132350884",
                Year = 2008,
                Pages = 464,
                Genre = "Programming",
                CreatedAt = seedDate,
                IsAvailable = true
            },
            new
            {
                Id = bookId2,
                Title = "The Pragmatic Programmer",
                AuthorId = authorId3,
                ISBN = "978-0135957059",
                Year = 2019,
                Pages = 352,
                Genre = "Programming",
                CreatedAt = seedDate,
                IsAvailable = true
            },
            new
            {
                Id = bookId3,
                Title = "Design Patterns",
                AuthorId = authorId2,
                ISBN = "978-0201633610",
                Year = 1994,
                Pages = 395,
                Genre = "Programming",
                CreatedAt = seedDate,
                IsAvailable = true
            }
        );
    }
}