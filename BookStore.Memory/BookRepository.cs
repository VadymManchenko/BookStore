using BookStore.Entities;
using BookStore.Interfaces;
using System.Linq;

namespace BookStore.Memory;

public class BookRepository : IBookRepository
{
    private readonly Book[] books = new[]
    {
        new Book(1, "Book1", "ISBN1234567891", "Author1", 500m, "description1"),
        new Book(2, "Book2","ISBN1234567892", "Author2", 700m, "description2"),
        new Book(3, "Book3", "ISBN1234567893", "Author3", 850m, "description3"),
    };

    public Book GetById(int id)
    {
        return books.Single(book => book.Id == id);
    }

    public Book[] GetAllByIds(IEnumerable<int> ids)
    {
        var foundBooks = from book in books
            join bookId in ids on book.Id equals bookId
            select book;
        
        return foundBooks.ToArray();
    }
    
    public Book[] GetAllByIsbn(string isbn)
    {
        return books.Where(book => book.Isbn == isbn).ToArray();
    }

    public Book[] GetAllByTitleOrAuthor(string titlePart)
    {
        return books.Where(
            book => book.Title
                .Contains(titlePart, StringComparison.InvariantCultureIgnoreCase)
                || book.Author
                .Contains(titlePart, StringComparison.InvariantCultureIgnoreCase))
            .ToArray();
    }
}