using BookStore.Entities;

namespace BookStore;

public class Order
{
    public int Id { get; }

    private List<OrderItem> items;

    public IReadOnlyCollection<OrderItem> Items => items;
    
    public int TotalCount
    {
        get { return items.Sum(item => item.Count); }
    }

    public decimal TotalPrice
    {
        get { return items.Sum(item => item.Price * item.Count); }
    }

    public Order(int id, IEnumerable<OrderItem> items)
    {
        if (items is null)
            throw new ArgumentNullException(nameof(items));
        
        Id = id;
        this.items = new List<OrderItem>(items);
    }

    public void AddItem(Book book, int count)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        var item = items.SingleOrDefault(books => books.BookId == book.Id);

        if (item is null)
        {
            items.Add(new OrderItem(book.Id, count, book.Price));
        }
        else
        {
            items.Remove(item);
            items.Add(new OrderItem(book.Id, item.Count + count, book.Price));
            
        }
    }
}