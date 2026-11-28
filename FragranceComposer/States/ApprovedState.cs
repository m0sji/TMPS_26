using FragranceComposer.Models;
namespace FragranceComposer.States;

public class ApprovedState : OrderState
{
    public override string Name => "Approved";
    public override void StartMaceration(ProductionOrder order) => order.TransitionTo(new MaceratingState());
    public override void Cancel(ProductionOrder order) => order.TransitionTo(new CancelledState());
   
}