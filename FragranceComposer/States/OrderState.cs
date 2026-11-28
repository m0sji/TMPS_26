using FragranceComposer.Models;
namespace FragranceComposer.States;

public abstract class OrderState : IOrderState
{
    public abstract string Name {get;}

    public virtual void Approve(ProductionOrder order) => throw Forbidden(order, "approve");
    public virtual void StartMaceration(ProductionOrder order) => throw Forbidden(order, "start maceration of");
    public virtual void Bottle(ProductionOrder order) => throw Forbidden(order, "bottle");
    public virtual void Ship(ProductionOrder order) => throw Forbidden(order, "ship");
    public virtual void Cancel(ProductionOrder order) => throw Forbidden(order, "cancel");

    private InvalidOperationException Forbidden(ProductionOrder order, string action) =>
     new($"Cannot {action} order {order.Id} while it is {Name}");
}