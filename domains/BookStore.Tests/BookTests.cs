using BookStore.Entities;

namespace BookStore.Tests;

public class BookTests
{
    [Fact]
    public void IsIsbn_WithNull_ReturnFalse()
    {
        bool actual = Book.IsIsbn(null);
        
        Assert.False(actual);
    }
    
    [Fact]
    public void IsIsbn_WithBlankString_ReturnFalse()
    {
        bool actual = Book.IsIsbn("  ");
        
        Assert.False(actual);
    }

    [Fact]
    public void IsIsbn_WithInvalidIsbn_ReturnFalse()
    {
        bool actual = Book.IsIsbn("ISBN 1");
        
        Assert.False(actual);
    }
    
    [Fact]
    public void IsIsbn_WithIsbn10_ReturnTrue()
    {
        bool actual = Book.IsIsbn("ISBN 112-415-125 5");
        
        Assert.True(actual);
    }
    
    [Fact]
    public void IsIsbn_WithIsbn13_ReturnTrue()
    {
        bool actual = Book.IsIsbn("ISBN 112-415-125 1488");
        
        Assert.True(actual);
    }
    
    [Fact]
    public void IsIsbn_WithTrashStart_ReturnFalse()
    {
        bool actual = Book.IsIsbn("hsdhsd ISBN 112-415-125 5");
        
        Assert.False(actual);
    }
}