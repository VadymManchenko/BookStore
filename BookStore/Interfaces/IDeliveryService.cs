using BookStore.Contractors;

namespace BookStore.Interfaces;

public interface IDeliveryService
{
    string UniqueCode { get; }

    string Title { get; }

    Form CreateForm(Order order);

    Form MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> values);
}