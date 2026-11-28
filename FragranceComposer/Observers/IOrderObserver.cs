using FragranceComposer.Models;
namespace FragranceComposer.Observers;

public interface IOrderObserver
{
    void OnStateChanged(ProductionOrder order, string previousState);
}