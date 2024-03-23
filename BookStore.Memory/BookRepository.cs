using BookStore.Entities;
using BookStore.Interfaces;
using System.Linq;

namespace BookStore.Memory;

public class BookRepository : IBookRepository
{
    private readonly Book[] books = new[]
    {
        new Book(1, "Book1", "ISBN1", "Author1"),
        new Book(2, "Book2","ISBN2", "Author2"),
        new Book(3, "Book3", "ISBN3", "Author3"),
    };

    public Book[] GetAllByIsbn(string isbn)
    {
        throw new NotImplementedException();
    }

    public Book[] GetAllByTitleOrAuthor(string titlePart)
    {
        return books.Where(
            book => book.Title
                .Contains(titlePart, StringComparison.InvariantCultureIgnoreCase))
            .ToArray();
    }
}