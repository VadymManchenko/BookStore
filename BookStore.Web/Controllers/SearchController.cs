using BookStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

public class SearchController : Controller
{
    private readonly IBookRepository bookRepository;
    // GET
    public SearchController(IBookRepository bookRepository)
    {
        this.bookRepository = bookRepository;
    }

    public IActionResult Index(string query)
    {
        var books = bookRepository.GetAllByTitle(query);
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