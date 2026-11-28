using FragranceComposer.Interfaces;
using FragranceComposer.Models;
namespace FragranceComposer.Approvals;

public class BottleStockHandler : ApprovalHandler
{
    private readonly IBottleInventory _inventory;

    public BottleStockHandler(IBottleInventory inventory)
    {
        _inventory = inventory;
    }

    public override ApprovalResult Handle(ProductionOrder order)
    {
        string bottle = order.Product.Bottle.Name;
        int available = _inventory.Available(bottle);
        if (available < order.Quantity)
        return ApprovalResult.Rejected($"stock check: needs {order.Quantity} x {bottle}, only {available} in stock");
        return base.Handle(order);
    }
}