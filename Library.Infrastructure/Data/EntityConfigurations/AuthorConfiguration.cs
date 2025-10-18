using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.EntityConfigurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Biography)
            .HasMaxLength(1000);

        builder.Property(e => e.Country)
            .HasMaxLength(50);

        builder.Property(e => e.MostPopularGenre)
            .HasMaxLength(50);

        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<Author> builder)
    {
        var authorId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var authorId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var authorId3 = Guid.Parse("33333333-3333-3333-3333-333333333333");

        builder.HasData(
            new Author(authorId1)
            {
                Name = "Robert C. Martin",
                Biography = "Software engineer and author, known for Clean Code",
                BirthDate = new DateTime(1952, 12, 5),
                Country = "USA",
                TotalBooksPublished = 1,
                MostPopularGenre = "Programming"
            },
            new Author(authorId2)
            {
                Name = "Gang of Four",
                Biography = "Erich Gamma, Richard Helm, Ralph Johnson, and John Vlissides",
                BirthDate = new DateTime(1960, 1, 1),
                Country = "Various",
                TotalBooksPublished = 1,
                MostPopularGenre = "Programming"
            },
            new Author(authorId3)
            {
                Name = "David Thomas & Andrew Hunt",
                Biography = "Authors of The Pragmatic Programmer",
                BirthDate = new DateTime(1965, 1, 1),
                Country = "USA",
                TotalBooksPublished = 1,
                MostPopularGenre = "Programming"
            }
        );
    }
}