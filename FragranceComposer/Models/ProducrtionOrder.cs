using FragranceComposer.States;
namespace FragranceComposer.Models;

public class ProductionOrder
{
    private IOrderState _state = new DraftState();

    public string Id {get;}
    public Product Product{get;}
    public int Quantity {get;}
    public string StateName => _state.Name;

    public ProductionOrder(string id, Product product, int quantity)
    {
        if (quantity <= 0 )
        throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive");
        Id= id;
        Product = product;
        Quantity = quantity;
    }

    public void Approve() => _state.Approve(this);
    public void StartMaceration() => _state.StartMaceration(this);
    public void Bottle() => _state.Bottle(this);
    public void Ship() => _state.Ship(this);
    public void Cancel() => _state.Cancel(this);

    internal void TransitionTo(IOrderState next)
    {
        _state = next;
    }



}