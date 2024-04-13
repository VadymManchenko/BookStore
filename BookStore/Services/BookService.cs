using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using BookStore.Entities;
using BookStore.Interfaces;

namespace BookStore.Services;

public class BookService
{
    private readonly IBookRepository _bookRepository;
    //private readonly Func<IDbConnection> dbConnection;
    //private readonly DbConnectionFactory _dbConnectionFactory;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    public Book[] GetAllByQuery(string query)
    {
        if (Book.IsIsbn(query))
            return _bookRepository.GetAllByIsbn(query);

        return _bookRepository.GetAllByTitleOrAuthor(query);
    }

    /*private Book[] GetAllByIsbn(string inputIsbn)
    {
        using var connection = dbConnection();
        using (var command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT Id, Title, Author, Isbn," +
                                  "Description, Price" +
                                  "FROM Books " +
                                  "WHERE Isbn = @Isbn";
            
            command.Parameters.Add(inputIsbn);

            using (var reader = command.ExecuteReader())
            {
                var result = new List<Book>();

                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var title = reader.GetString(1);
                    var author = reader.GetString(2);
                    var isbn = reader.GetString(3);
                    var description = reader.GetString(4);
                    var price = reader.GetDecimal(5);
                    
                    result.Add(new Book(id, title, isbn, author, price, description));
                }

                return result.ToArray();
            }
        }
    }*/
    
}