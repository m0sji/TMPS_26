using FragranceComposer.Models;
namespace FragranceComposer.States;

public class DraftState : OrderState
{
    public override string Name => "Draft";
    public override void Approve(ProductionOrder order)
    {
        if ( order.RequestApproval().IsApproved)
        order.TransitionTo(new ApprovedState());
    }

    public override void Cancel(ProductionOrder order) => order.TransitionTo(new CancelledState());
}