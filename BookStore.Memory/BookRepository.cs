using BookStore.Entities;
using BookStore.Interfaces;
using System.Linq;

namespace BookStore.Memory;

public class BookRepository : IBookRepository
{
    private readonly Book[] books = new[]
    {
        new Book(1, "Book1"),
        new Book(2, "Book2"),
        new Book(3, "Book3"),
    };

    public Book[] GetAllByTitle(string titlePart)
    {
        return books.Where(
            book => book.Title
                .Contains(titlePart)).ToArray();
    }
}