using BookStore.Entities;

namespace BookStore.Interfaces;

public interface IBookRepository
{
    Book GetById(int id);

    Book[] GetAllByIds(IEnumerable<int> ids);
    
    Book[] GetAllByIsbn(string isbn);
    
    //Book[] GetAllByAuthor(string author);
    
    Book[] GetAllByTitleOrAuthor(string titlePart);
}