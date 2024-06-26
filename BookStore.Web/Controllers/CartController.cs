using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Web;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

public class CartController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IOrderRepository _orderRepository;

    public CartController(IBookRepository bookRepository, IOrderRepository orderRepository)
    {
        _bookRepository = bookRepository;
        _orderRepository = orderRepository;
    }

    public IActionResult Add(int id)
    {
        Order order;
        Cart cart;

        if (HttpContext.Session.TryGetCart(out cart))
        {
            order = _orderRepository.GetById(cart.OrderId);
        }
        else
        {
            order = _orderRepository.Create();
            cart = new Cart(order.Id);
        }
        
        var book = _bookRepository.GetById(id);
        
        order.AddItem(book, 1);
        _orderRepository.Update(order);

        cart.TotalCount = order.TotalCount;
        cart.TotalPrice = order.TotalPrice;

        HttpContext.Session.Set(cart);
        
        return RedirectToAction("Index", "Book", new { id });
    }
}