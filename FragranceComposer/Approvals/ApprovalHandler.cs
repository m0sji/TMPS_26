using FragranceComposer.Models;
namespace FragranceComposer.Approvals;

public abstract class ApprovalHandler : IApprovalHandler
{
    private IApprovalHandler? _next;
    public IApprovalHandler SetNext(IApprovalHandler next)
    {
        _next = next;
        return next;
    }

    public virtual ApprovalResult Handle(ProductionOrder order) =>
     _next is not null
     ? _next.Handle(order)
     : ApprovalResult.Rejected($"no handler in the chain approved {order.TotalCost:F2} EUR");
}