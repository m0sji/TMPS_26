using FragranceComposer.Models;
namespace FragranceComposer.Interfaces;

public interface IScent
{
    string Name {get;}
    IReadOnlyDictionary<Ingredient, double> Composition {get;}
    decimal CostPerMl {get;}
}