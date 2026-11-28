using FragranceComposer.Models;
namespace FragranceComposer.Observers;

public class CustomerNotifier : IOrderObserver
{
    private readonly string _customer;
    private readonly List<string> _messages = new();

    public CustomerNotifier(string customer)
    {
        _customer = customer;
    }

    public IReadOnlyList<string> Messages => _messages;

    public void OnStateChanged(ProductionOrder order, string previousState)
    {
        if (order.StateName != "Shipped")
        return;
        string message = $"to {_customer}: your {order.Quantity} x {order.Product.Scent.Name} have been shipped";
        _messages.Add(message);
        Console.WriteLine($"  [customer] {message}");
    }
}