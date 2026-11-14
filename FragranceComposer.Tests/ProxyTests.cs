using FragranceComposer.Builders;
using FragranceComposer.Composites;
using FragranceComposer.Concentrations;
using FragranceComposer.Interfaces;
using FragranceComposer.Models;
using FragranceComposer.Proxies;
using FragranceComposer.Repositories;
using FragranceComposer.Services;
namespace FragranceComposer.Tests;

public class ProxyTests
{
    private readonly IScent _real;
    private readonly UserSession _session = new();
    private readonly IScent _proxy;

    public ProxyTests()
    {
        var director =new FragranceDirector(new FragranceBuilder(new InMemoryIngredientRepository()));
        _real = director.BuildGourmand("Secret", new EauDeParfum());
        _proxy = new ProtectedScent(_real, _session);
    }

    [Fact]
    public void Customer_CanReadNameAndCost_ButNotComposition()
    {
        _session.SignIn(UserRole.Customer);
        Assert.Equal(_real.Name, _proxy.Name);
        Assert.Equal(_real.CostPerMl, _proxy.CostPerMl);
        Assert.Throws<UnauthorizedAccessException>(()=> _proxy.Composition);
    }

    [Fact]
    public void Perfumer_GetsTheRealCompositionUnchanged()
    {
        _session.SignIn(UserRole.Perfumer);
        Assert.Same(_real.Composition, _proxy.Composition);
    }

    [Fact]
    public void CompositeContainingProxy_HidesCompositionButStillGivesCost()
    {
        IScent layered = new LayeredScent("Layered").Add(_proxy, 1).Add(_real, 1);
        _session.SignIn(UserRole.Customer);
        Assert.Equal(_real.CostPerMl, layered.CostPerMl);
        Assert.Throws<UnauthorizedAccessException>(()=> layered.Composition);

        _session.SignIn(UserRole.Perfumer);
        Assert.Equal(100, layered.Composition.Values.Sum(), 6);
    
    }
}
