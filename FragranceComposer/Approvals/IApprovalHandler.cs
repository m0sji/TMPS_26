using FragranceComposer.Models;
namespace FragranceComposer.Approvals;

public interface IApprovalHandler
{
    IApprovalHandler SetNext(IApprovalHandler next);
    ApprovalResult Handle(ProductionOrder order);
}