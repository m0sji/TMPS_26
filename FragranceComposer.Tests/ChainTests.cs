using FragranceComposer.Approvals;
using FragranceComposer.Builders;
using FragranceComposer.Factories;
using FragranceComposer.Models;
using FragranceComposer.Repositories;

namespace FragranceComposer.Tests;

public class ChainTests
{
    private readonly Product _aquatic;
    private readonly Product _gourmand;

    public ChainTests()
    {
        IProductLineFactory line = new ClassicLineFactory();
        var director = new FragranceDirector(new FragranceBuilder(new InMemoryIngredientRepository()));
        _aquatic = new Product(line.LineName, director.BuildAquatic("Aquatic", line.CreateConcentration()),
        line.CreateBottle(), line.CreatePackaging());
        _gourmand = new Product(line.LineName, director.BuildGourmand("Gourmand", line.CreateConcentration()),
        line.CreateBottle(), line.CreatePackaging());
    }

    [Fact]
    public void Request_IsApprovedByTheFirstApproverWhoseLimitCoversTheCost()
    {
        decimal cost = 10 * _aquatic.Cost;
        IApprovalHandler chain = new CostApprovalHandler("Perfumer", cost -1);
        chain.SetNext(new CostApprovalHandler("Lab manager", cost+1))
        .SetNext(new CostApprovalHandler("Director", cost *10));
        var order = new ProductionOrder("PO-1", _aquatic, 10, chain);

        order.Approve();

        Assert.Equal("Approved", order.StateName);
        Assert.StartsWith("approved by Lab manager", order.ApprovalNote);
    }

    [Fact]
    public void UnhandledRequest_EndsTheChainSafely_AndTheOrderStaysDraft()
    {
        decimal cost = 10* _aquatic.Cost;
        IApprovalHandler chain = new CostApprovalHandler("Perfumer", cost -1);
        var order = new ProductionOrder("PO-1", _aquatic, 10, chain);

        order.Approve();
        Assert.Equal("Draft", order.StateName);
        Assert.StartsWith("no handler in the chain approved", order.ApprovalNote);
    }

    [Fact]
    public void SafetyCheck_RejectsBeforeAnyApproverIsAsked()
    {
        IApprovalHandler chain = new SafetyLimitHandler(new Dictionary<string, double>{ ["Jasmine"]= 20});
        chain.SetNext(new CostApprovalHandler("Director", decimal.MaxValue));
        var risky = new ProductionOrder("PO-1", _gourmand, 10, chain);
        var safe = new ProductionOrder("PO-2", _aquatic, 10, chain);

        risky.Approve();
        safe.Approve();

        Assert.Equal("Draft", risky.StateName);
        Assert.StartsWith("safety check: Jasmine", risky.ApprovalNote);
        Assert.Equal("Approved", safe.StateName);
    }

    [Fact]
    public void StockCheck_RefectsAnOrderLargerThanTheBottleStock()
    {
        IApprovalHandler chain = new BottleStockHandler(new InMemoryBottleInventory());
        chain.SetNext(new CostApprovalHandler("Director", decimal.MaxValue));
        var order = new ProductionOrder("PO-1", _aquatic,121, chain);

        order.Approve();

        Assert.Equal("Draft", order.StateName);
        Assert.Equal("stock check: needs 121 x Glass bottle, only 120 in stock", order.ApprovalNote);
    }
}