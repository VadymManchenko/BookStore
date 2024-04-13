namespace BookStore.Tests;

public class OrderItemTests
{
    [Fact]
    public void OrderItem_WithZeroCount_ThrowsOutOfRangeException()
    {
        int count = 0;
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new OrderItem(1, count, 10m));
    }
    [Fact]
    public void OrderItem_WithNegativeCount_ThrowsOutOfRangeException()
    {
        int count = -1;
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new OrderItem(1, count, 10m));
    }
    [Fact]
    public void OrderItem_WithPositiveCount_SetsCount()
    {
        var orderItem = new OrderItem(1, 2, 3m);
        
        Assert.Equal(1, orderItem.BookId);
        Assert.Equal(2, orderItem.Count);
        Assert.Equal(3m, orderItem.Price);
    }
}