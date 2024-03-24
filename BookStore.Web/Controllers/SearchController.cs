using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

public class SearchController : Controller
{
    private readonly BookService bookService;

    // GET
    public SearchController(BookService bookService)
    {
        this.bookService = bookService;
    }

    public IActionResult Index(string query)
    {
        var books = bookService.GetAllByQuery(query);
        return View(books);
    }

    public IActionResult Create()
    {
        throw new NotImplementedException();
    }

    public IActionResult Edit()
    {
        throw new NotImplementedException();
    }

    public IActionResult Details()
    {
        throw new NotImplementedException();
    }

    public IActionResult Delete()
    {
        throw new NotImplementedException();
    }
}