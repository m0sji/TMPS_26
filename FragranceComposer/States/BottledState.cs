using FragranceComposer.Models;
namespace FragranceComposer.States;

public class BottledState : OrderState
{
    public override string Name => "Bottled";
    public override void Ship(ProductionOrder order) => order.TransitionTo(new ShippedState());
}