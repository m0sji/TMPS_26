using FragranceComposer.Builders;
using FragranceComposer.Composites;
using FragranceComposer.Concentrations;
using FragranceComposer.Interfaces;
using FragranceComposer.Repositories;
using Xunit.Sdk;
namespace FragranceComposer.Tests;

public class CompositeTests
{
    private readonly IIngredientRepository _repository = new 
    InMemoryIngredientRepository();
    private readonly IScent _aquatic;
    private readonly IScent _gourmand;

    public CompositeTests()
    {
        var director = new FragranceDirector(new FragranceBuilder(_repository));
        _aquatic = director.BuildAquatic("Aquatic", new EauDeParfum());
        _gourmand = director.BuildGourmand("Gourmand", new EauDeParfum());
    }

    //Client code that knows only the COmponent interface
    private static double CompositionTotal(IScent scent) =>
    scent.Composition.Values.Sum();
    [Fact]
    public void LeafAndComposite_AreUsedThroughTheSameInterface()
    {
        IScent leaf = _aquatic;
        IScent composite = new LayeredScent("Layered").Add(_aquatic, 2).Add(_gourmand, 1);

        Assert.Equal(100, CompositionTotal(leaf), 6);
        Assert.Equal(100, CompositionTotal(composite), 6);
    }

    [Fact]
    public void Composite_WeightsCompositionAndCostBySprays()
    {
        IScent layered = new LayeredScent("Layered").Add(_aquatic, 2).Add(_gourmand, 1);
    

    //Sea Notes: only in aquatic (20%) -> 20*2/3
    //Bargamot: in both (15% and 15%) -> merged into one entry, 15
    //Vanilla: only in gourmand (40%) -> 40*1/3
    Assert.Equal(20.0*2/3, layered.Composition[_repository.GetByName("Sea Notes")],6);
    Assert.Equal(15.0, layered.Composition[_repository.GetByName("Bergamot")], 6);
    Assert.Equal(40.0/3, layered.Composition[_repository.GetByName("Vanilla")], 6);
    decimal expectedCost = (_aquatic.CostPerMl * 2 +_gourmand.CostPerMl * 1) /3;
    Assert.Equal(expectedCost, layered.CostPerMl, 10);
    }

    [Fact]
    public void Composite_CanContainAnotherComposite()
    {
        IScent inner = new LayeredScent("Inner").Add(_aquatic,1).Add(_gourmand, 1);
        IScent outer = new LayeredScent("Outer").Add(inner, 1).Add(_aquatic, 1);

        //aquatic share = 1/2 * 1/2 +1/2 =3/4, gournand share = 1/4
        decimal expectedCost = _aquatic.CostPerMl *0.75m +_gourmand.CostPerMl *0.25m;
        Assert.Equal(expectedCost, outer.CostPerMl, 10);
        Assert.Equal(100, CompositionTotal(outer), 6);
    }

    [Fact]
    public void EmptyComposite_Throws()
    {
        IScent empty = new LayeredScent("Empty");

        Assert.Throws<InvalidOperationException>(() => empty.CostPerMl);
        Assert.Throws<InvalidOperationException>(() => empty.Composition);
    }
}