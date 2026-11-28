using FragranceComposer.Interfaces;
using FragranceComposer.Models;
namespace FragranceComposer.Observers;

public class BottleStockObserver : IOrderObserver
{
    private readonly IBottleInventory _inventory;

    public BottleStockObserver(IBottleInventory inventory)
    {
        _inventory = inventory;
    }

    public void OnStateChanged(ProductionOrder order, string previousState)
    {
        if(order.StateName != "Bottled")
        return;
        string bottle = order.Product.Bottle.Name;
        _inventory.Take(bottle, order.Quantity);
        Console.WriteLine($"  [stock] took {order.Quantity} x {bottle}, {_inventory.Available(bottle)} left");
    }
}