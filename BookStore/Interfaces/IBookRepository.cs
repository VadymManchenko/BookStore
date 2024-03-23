using BookStore.Entities;

namespace BookStore.Interfaces;

public interface IBookRepository
{
    Book[] GetAllByIsbn(string isbn);
    //Book[] GetAllByAuthor(string author);
    Book[] GetAllByTitleOrAuthor(string titlePart);
}