using BookStore.Entities;

namespace BookStore.Interfaces;

public interface IBookRepository
{
    Book[] GetAllByTitle(string title);
}