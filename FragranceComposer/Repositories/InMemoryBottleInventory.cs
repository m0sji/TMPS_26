using FragranceComposer.Interfaces;
namespace FragranceComposer.Repositories;

public class InMemoryBottleInventory : IBottleInventory
{
    private readonly Dictionary<string, int> _stock = new()
    {
        ["Crystal bottle"]=30,
        ["Glass bottle"]=120,
        ["Travel spray bottle"]= 500
    };

    public int Available(string bottleName) => _stock.GetValueOrDefault(bottleName);
}