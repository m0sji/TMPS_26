using FragranceComposer.Interfaces;
namespace FragranceComposer.Models;

public class Fragrance
{
    public string Name { get; }
    public ScentFamily Family {get;}
    public IConcentration Concentration { get; }
    public IReadOnlyDictionary<Ingredient, double> Formula { get; }

    public Fragrance(string name,ScentFamily family, IConcentration concentration, IReadOnlyDictionary<Ingredient, double> formula)
    {
        Name = name;
        Family = family;
        Concentration = concentration;
        Formula = formula;
    }
}