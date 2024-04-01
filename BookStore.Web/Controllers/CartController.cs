using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Web;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

public class CartController : Controller
{
    private readonly IBookRepository _bookRepository;

    public CartController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public IActionResult Add(int id)
    {
        var book = _bookRepository.GetById(id);
        if (!HttpContext.Session.TryGetCart(out var cart))
        {
            cart = new Cart();
        }

        if (cart != null && cart.Items.ContainsKey(id)) 
            cart.Items[id]++;
        
        else if (cart != null) cart.Items[id] = 1;


        if (cart != null)
        {
            cart.Amount += book.Price;
            HttpContext.Session.Set(cart);
        }

        return RedirectToAction("Index", "Book", new { id });
    }
}