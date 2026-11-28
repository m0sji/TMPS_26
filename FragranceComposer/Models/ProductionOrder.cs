using System.Numerics;
using FragranceComposer.Approvals;
using FragranceComposer.States;
namespace FragranceComposer.Models;

public class ProductionOrder
{
    private readonly IApprovalHandler _approvalChain;
    private IOrderState _state = new DraftState();

    public string Id {get;}
    public Product Product{get;}
    public int Quantity {get;}
    public string StateName => _state.Name;
    public decimal TotalCost => Quantity * Product.Cost;
    public string? ApprovalNote {get; private set; }

    public ProductionOrder(string id, Product product, int quantity, IApprovalHandler approvalChain)
    {
        if (quantity <= 0 )
        throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive");
        Id = id;
        Product = product;
        Quantity = quantity;
        _approvalChain = approvalChain;
    }

    public void Approve() => _state.Approve(this);
    public void StartMaceration() => _state.StartMaceration(this);
    public void Bottle() => _state.Bottle(this);
    public void Ship() => _state.Ship(this);
    public void Cancel() => _state.Cancel(this);

    internal ApprovalResult RequestApproval()
    {
        ApprovalResult result = _approvalChain.Handle(this);
        ApprovalNote = result.Message;
        return result;
    }

    internal void TransitionTo(IOrderState next)
    {
        _state = next;
    }
}