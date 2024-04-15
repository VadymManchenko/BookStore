namespace BookStore.Models;

public class OrderModel
{
    public int Id { get; set; }

    public OrderItemModel[] Items { get; set; } = Array.Empty<OrderItemModel>();

    public int TotalCount { get; set; }

    public decimal TotalPrice { get; set; }

}