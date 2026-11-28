using System.Runtime.CompilerServices;
using FragranceComposer.Approvals;
using FragranceComposer.Builders;
using FragranceComposer.Factories;
using FragranceComposer.Models;
using FragranceComposer.Observers;
using FragranceComposer.Repositories;

namespace FragranceComposer.Tests;

public class ObserverTests
{
    private readonly Product _product;
    private readonly IApprovalHandler _approveAll = new CostApprovalHandler("Test approver", decimal.MaxValue);

    public ObserverTests()
    {
        IProductLineFactory line = new ClassicLineFactory();
        var director = new FragranceDirector(new FragranceBuilder(new InMemoryIngredientRepository()));
        _product = new Product(line.LineName, director.BuildAquatic("Aquatic", line.CreateConcentration()), line.CreateBottle(),line.CreatePackaging());
    }

    [Fact]
    public void SubscribedObserversAreNotified_AndUnsubscribedOnesAreNot()
    {
        var order = new ProductionOrder("PO-1", _product, 10, _approveAll);
        var first = new AuditLogObserver();
        var second = new AuditLogObserver();
        order.Subscribe(first);
        order.Subscribe(second);

        order.Approve();
        order.Unsubscribe(second);
        order.StartMaceration();

        Assert.Equal(new[] { "PO-1: Draft -> Approved", "PO-1: Approved -> Macerating" }, first.Entries);
        Assert.Equal(new[] { "PO-1: Draft -> Approved" }, second.Entries);
    }

    [Fact]
    public void RejectedApproval_ChangesNoState_AndNotifiesNobody()
    {
        var order = new ProductionOrder("PO-1", _product, 10 , new CostApprovalHandler("Perfumer", 0m));
        var audit = new AuditLogObserver();
        order.Subscribe(audit);

        order.Approve();

        Assert.Equal("Draft", order.StateName);
        Assert.Empty(audit.Entries);
    }

    [Fact]
    public void CustomerIsNotrifiedOnlyWhenTheOrderIsShipped()
    {
        var order = new ProductionOrder("PO-1", _product, 10 , _approveAll);
        var customer = new CustomerNotifier("Shop");
        order.Subscribe(customer);

        order.Approve();
        order.StartMaceration();
        order.Bottle();
        Assert.Empty(customer.Messages);

        order.Ship();
        Assert.Single(customer.Messages);
    }

    [Fact]
    public void BottleOrder_TakesBottlesFromStock_SoTheStockCheckRejectsTheNextOrder()
    {
        var inventory = new InMemoryBottleInventory();
        IApprovalHandler chain = new BottleStockHandler(inventory);
        chain.SetNext(new CostApprovalHandler("Director", decimal.MaxValue));
        var first = new ProductionOrder("PO-1", _product, 100, chain);
        first.Subscribe(new BottleStockObserver(inventory));

        first.Approve();
        first.StartMaceration();
        first.Bottle();
        var second = new ProductionOrder("PO-2", _product, 30, chain);
        second.Approve();

        Assert.Equal(20, inventory.Available("Glass bottle"));
        Assert.Equal("Draft", second.StateName);
        Assert.Equal("stock check: needs 30 x Glass bottle, only 20 in stock", second.ApprovalNote);
    }
}