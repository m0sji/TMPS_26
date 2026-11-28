using FragranceComposer.Models;
namespace FragranceComposer.States;

public interface IOrderState
{
    string Name {get;}
    void Approve(ProductionOrder order);
    void StartMaceration(ProductionOrder order);
    void Bottle(ProductionOrder order);
    void Ship(ProductionOrder order);
    void Cancel(ProductionOrder order);
}