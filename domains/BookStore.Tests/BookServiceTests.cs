using BookStore.Entities;
using BookStore.Interfaces;
using BookStore.Services;
using Moq;

namespace BookStore.Tests;

public class BookServiceTests
{
    const int IdOfIsbnSearch = 1;
    const int IdOfAuthorSearch = 2;
    [Fact]
    public void GetAllByQuery_WithIsbn_CallsMethod()
    {
        var bookRepositoryStub = new Mock<IBookRepository>();
        bookRepositoryStub.Setup(
                e => e.GetAllByIsbn(
                    It.IsAny<string>()))
            .Returns(new[] { new Book(IdOfIsbnSearch, "", "", "", 0m, "") });

        bookRepositoryStub.Setup(
                e => e.GetAllByTitleOrAuthor(
                    It.IsAny<string>()))
            .Returns(new[] { new Book(IdOfAuthorSearch, "", "", "", 0m, "") });

        var bookService = new BookService(bookRepositoryStub.Object);

        var actual = bookService.GetAllByQuery("ISBN 124-124-643 3");
        
        Assert.Collection(actual, book => Assert.Equal(IdOfIsbnSearch, book.Id));
    }
    [Fact]
    public void GetAllByQuery_WithAuthor_CallsMethod()
    {
        var bookRepositoryStub = new Mock<IBookRepository>();
        bookRepositoryStub.Setup(
                e => e.GetAllByIsbn(
                    It.IsAny<string>()))
            .Returns(new[] { new Book(IdOfIsbnSearch, "", "", "", 0m, "") });

        bookRepositoryStub.Setup(
                e => e.GetAllByTitleOrAuthor(
                    It.IsAny<string>()))
            .Returns(new[] { new Book(IdOfAuthorSearch, "", "", "",0m, "") });

        var bookService = new BookService(bookRepositoryStub.Object);

        var actual = bookService.GetAllByQuery("124-124-643 3");
        
        Assert.Collection(actual, book => Assert.Equal(IdOfAuthorSearch, book.Id));
    }
}