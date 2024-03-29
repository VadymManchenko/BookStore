using System.Text.RegularExpressions;

namespace BookStore.Entities;

public class Book
{
    public int Id { get; }
    public string Isbn { get; }
    public string Author { get; }
    public string Title { get; }
    public decimal Price { get; }
    public string Description { get; }

    public Book(int id, string title) : this(id, title, null, null, 0m, null)
    {
        Id = id;
        Title = title;
    }
    public Book(int id, string title, string isbn, string author, decimal price, string description)
    {
        Id = id;
        Title = title;
        Isbn = isbn;
        Author = author;
        Price = price;
        Description = description;
    }

    internal static bool IsIsbn(string? s)
    {
        if (s is null)
        {
            return false;
        }

        s = s.Replace("-", "")
            .Replace(" ", "")
            .ToUpper();
        
        return Regex.IsMatch(s, "^ISBN\\d{10}(\\d{3})?$");
    }
}