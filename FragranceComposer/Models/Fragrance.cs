using System.ComponentModel;
using FragranceComposer.Interfaces;
namespace FragranceComposer.Models;

public class Fragrance : IPrototype<Fragrance>
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

    public Fragrance Clone()=>CloneAs(Name);
    public Fragrance CloneAs(string name, IConcentration? concentration = null)=>
    new Fragrance(name, Family, concentration ?? Concentration, new Dictionary<Ingredient, double>(Formula));
}