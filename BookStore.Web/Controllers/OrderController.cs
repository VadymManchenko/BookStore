using System.Net;
using System.Text.RegularExpressions;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Web;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBookRepository _bookRepository;
    private readonly INotificationService _notificationService;
    private readonly IEnumerable<IDeliveryService> _deliveryService;

    public OrderController(IOrderRepository orderRepository, 
        IBookRepository bookRepository, INotificationService notificationService, 
        IEnumerable<IDeliveryService> deliveryService)
    {
        _orderRepository = orderRepository;
        _bookRepository = bookRepository;
        _notificationService = notificationService;
        _deliveryService = deliveryService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (HttpContext.Session.TryGetCart(out Cart cart))
        {
            var order = _orderRepository.GetById(cart.OrderId);
            OrderModel model = Map(order);

            return View(model);
        }

        return View("Empty");
    }
    
    private OrderModel Map(Order order)
    {
        var bookIds = order.Items.Select(item => item.BookId);
        var books = _bookRepository.GetAllByIds(bookIds);
        var itemModels = from item in order.Items
            join book in books on item.BookId equals book.Id
            select new OrderItemModel
            {
                BookId = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = item.Price,
                Count = item.Count,
            };
        return new OrderModel
        {
            Id = order.Id,
            Items = itemModels.ToArray(),
            TotalCount = order.TotalCount,
            TotalPrice = order.TotalPrice,
        };
    }

    [HttpPost]
    public IActionResult AddItem(int id)
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

    [HttpPost]
    public IActionResult UpdateItem(int bookId, int count)
    {
        (Order order, Cart cart) = GetOrCreateOrderAndCart();
        order.GetItem(bookId).Count = count;

        SaveOrderAndCart(order, cart);
        
        return RedirectToAction("Index", "Order");
    }
    
    private (Order order, Cart cart) GetOrCreateOrderAndCart()
    {
        Order order;
        if (HttpContext.Session.TryGetCart(out Cart cart))
        {
            order = _orderRepository.GetById(cart.OrderId);
        }
        else
        {
            order = _orderRepository.Create();
            cart = new Cart(order.Id);
        }
        return (order, cart);
    }
    private void SaveOrderAndCart(Order order, Cart cart)
    {
        _orderRepository.Update(order);
        cart.TotalCount = order.TotalCount;
        cart.TotalPrice = order.TotalPrice;
        HttpContext.Session.Set(cart);
    }

    
    [HttpPost]
    public IActionResult RemoveItem(int bookId)
    {
        (Order order, Cart cart) = GetOrCreateOrderAndCart();
        order.RemoveItem(bookId);
        SaveOrderAndCart(order, cart);

        return RedirectToAction("Index", "Order");
    }
    
    [HttpPost]
    public IActionResult SendConfirmationCode(int id, string cellPhone)
    {
        var order = _orderRepository.GetById(id);
        var model = Map(order);

        if (!IsValidCellPhone(cellPhone))
        {
            model.Errors["cellPhone"] = "Номер телефона не соответствует формату +79876543210";
            return View("Index", model);
        }

        int code = 1111; // random.Next(1000, 10000)
        HttpContext.Session.SetInt32(cellPhone, code);
        _notificationService.SendConfirmationCode(cellPhone, code);

        return View("Confirmation",
            new ConfirmationModel
            {
                OrderId = id,
                CellPhone = cellPhone
            });
    }

    private bool IsValidCellPhone(string cellPhone)
    {
        if (cellPhone is null)
            return false;

        cellPhone = cellPhone.Replace(" ", "")
            .Replace("-", "");

        return Regex.IsMatch(cellPhone, @"^\+?\d{11}$");
    }
    
    [HttpPost]
    public IActionResult Confirmate(int id, string cellPhone, int code)
    {
        int? storedCode = HttpContext.Session.GetInt32(cellPhone);
        if (storedCode == null)
        {
            return View("Confirmation",
                new ConfirmationModel
                {
                    OrderId = id,
                    CellPhone = cellPhone,
                    Errors = new Dictionary<string, string>
                    {
                        { "code", "Empty code, try again" }
                    },
                }); ;
        }

        if (storedCode != code)
        {
            return View("Confirmation",
                new ConfirmationModel
                {
                    OrderId = id,
                    CellPhone = cellPhone,
                    Errors = new Dictionary<string, string>
                    {
                        { "code", "This code doesn`t match with correct code!" }
                    },
                }); ;
        }

        HttpContext.Session.Remove(cellPhone);

        var model = new DeliveryModel
        {
            OrderId = id,
            Methods = _deliveryService.ToDictionary(service => service.UniqueCode,
                service => service.Title),
        };

        return View("DeliveryMethod", model);
    }
    
    [HttpPost]
    public IActionResult StartDelivery(int id, string uniqueCode)
    {
        var deliveryService = _deliveryService.Single(service => service.UniqueCode == uniqueCode);
        var order = _orderRepository.GetById(id);

        var form = deliveryService.CreateForm(order);

        return View("DeliveryStep", form);
    }

    [HttpPost]
    public IActionResult NextDelivery(int id, string uniqueCode, int step, Dictionary<string, string> values)
    {
        var deliveryService = _deliveryService.Single(service => service.UniqueCode == uniqueCode);

        var form = deliveryService.MoveNext(id, step, values);

        if (form.IsFinal)
        {
            return null;
        }

        return View("DeliveryStep", form);
    }
}