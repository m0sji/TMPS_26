using FragranceComposer.Models;
namespace FragranceComposer.States;

public class MaceratingState : OrderState
{
    public override string Name => "Macerating";
    public override void Bottle(ProductionOrder order) => order.TransitionTo(new BottledState());
}