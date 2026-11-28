using FragranceComposer.Models;
namespace FragranceComposer.Approvals;

public class CostApprovalHandler : ApprovalHandler
{
    private readonly string _approver;
    private readonly decimal _limit;

    public CostApprovalHandler(string approver, decimal limit)
    {
        _approver = approver;
        _limit = limit;
    }

    public override ApprovalResult Handle(ProductionOrder order)
    {
        if (order.TotalCost <= _limit)
        return ApprovalResult.Approved($"approved by {_approver}: {order.TotalCost:F2} EUR is within the {_limit:F2} EUR limit");
        return base.Handle(order);
    }
}