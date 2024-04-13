using BookStore.Interfaces;

namespace BookStore.Memory;

public class OrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();
    
    public Order Create()
    {
        int nextId = _orders.Count + 1;
        var order = new Order(nextId, new OrderItem[0]);

        _orders.Add(order);
        return order;
    }

    public Order GetById(int id)
    {
        return _orders.Single(order => order.Id == id);
    }

    public void Update(Order order) { }
}