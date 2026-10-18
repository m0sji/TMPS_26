using FragranceComposer.Interfaces;
namespace FragranceComposer.Models;

public class Fragrance
{
    public string Name { get; }
    public IConcentration Concentration { get; }
    public Dictionary<Ingredient, double> Formula { get; }

    public Fragrance(string name, IConcentration concentration, Dictionary<Ingredient, double> formula)
    {
        Name = name;
        Concentration = concentration;
        Formula = formula;
    }
}