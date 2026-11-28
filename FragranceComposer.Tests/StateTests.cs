using System.Reflection;
using FragranceComposer.Builders;
using FragranceComposer.Factories;
using FragranceComposer.Models;
using FragranceComposer.Repositories;

namespace FragranceComposer.Tests;

public class StateTests
{
    private readonly Product _product;

    public StateTests()
    {
        IProductLineFactory line = new ClassicLineFactory();
        var director = new FragranceDirector(new FragranceBuilder(new InMemoryIngredientRepository()));
        var scent = director.BuildAquatic("Aquatic", line.CreateConcentration());
        _product = new Product(line.LineName, scent, line.CreateBottle(), line.CreatePackaging());
    }

    [Fact]
    public void ValidTransitions_FollowTheLifecycleUntilShipped()
    {
        var order = new ProductionOrder("PO-1", _product, 10);
        Assert.Equal("Draft", order.StateName);
        order.Approve();
        Assert.Equal("Approved", order.StateName);
        order.StartMaceration();
        Assert.Equal("Macerating", order.StateName);
        order.Bottle();
        Assert.Equal("Bottled", order.StateName);
        order.Ship();
        Assert.Equal("Shipped", order.StateName);
    }

    [Fact]
    public void ForbiddenTransition_IsRejected_AndStateDoesNotChange()
    {
        var order = new ProductionOrder("PO-1", _product, 10);
        Assert.Throws<InvalidOperationException>(() => order.Ship());
        Assert.Equal("Draft", order.StateName);

        order.Approve();
        order.StartMaceration();
        Assert.Throws<InvalidOperationException>(() => order.Cancel());
        Assert.Equal("Macerating", order.StateName);
    }

    [Fact]
    public void CancelledOrder_RejectsEveryAction()
    {
        var order = new ProductionOrder("PO-1", _product, 10);
        order.Cancel();
        Assert.Equal("Cancelled", order.StateName);

        Assert.Throws<InvalidOperationException>(() => order.Approve());
        Assert.Throws<InvalidOperationException>(() => order.StartMaceration());
        Assert.Throws<InvalidOperationException>(() => order.Bottle());
        Assert.Throws<InvalidOperationException>(() => order.Ship());
        Assert.Throws<InvalidOperationException>(() => order.Cancel());
        Assert.Equal("Cancelled", order.StateName);
    }
}