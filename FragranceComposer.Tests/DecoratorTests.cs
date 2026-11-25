using FragranceComposer.Builders;
using FragranceComposer.Composites;
using FragranceComposer.Decorators;
using FragranceComposer.Factories;
using FragranceComposer.Interfaces;
using FragranceComposer.Models;
using FragranceComposer.Proxies;
using FragranceComposer.Repositories;
using FragranceComposer.Services;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;

namespace FragranceComposer.Tests;

public class DecoratorTests
{
    private readonly IProductLineFactory _line = new ClassicLineFactory();
    private readonly IScent _scent;
    private readonly IProduct _product;

    public DecoratorTests()
    {
        var director = new FragranceDirector(new FragranceBuilder(new InMemoryIngredientRepository()));
        _scent = director.BuildAquatic("Aquatic", _line.CreateConcentration());
        _product = new Product(_line.LineName, _scent, _line.CreateBottle(), _line.CreatePackaging());
    }

    [Fact]
    public void Decorators_StackedInAnyOrder_GiveTheSameCost()
    {
        IProduct a = new SampleVialDecorator(new EngravingDecorator(new GiftWrapDecorator(_product), "Ana"),_scent);
        IProduct b = new GiftWrapDecorator(new SampleVialDecorator(new EngravingDecorator(_product, "Ana"),_scent));

        decimal expected = _product.Cost + 3.5m +6.0m + (2*_scent.CostPerMl + 0.6m);
        Assert.Equal(expected, a.Cost);
        Assert.Equal(expected, b.Cost);
        foreach (var addOn in new[] { "gift wrap", "engraving \"Ana\"", "sample of Aquatic" })
        {
            Assert.Contains(addOn, a.Description);
            Assert.Contains(addOn, b.Description);
        }
    }

    [Fact]
    public void Decorator_AddsItsOwnPriceAndTextToTheWrappedProduct()
    {
        IProduct wrapped = new GiftWrapDecorator(_product);

        Assert.Equal(_product.Cost +3.5m, wrapped.Cost);
        Assert.StartsWith(_product.Description, wrapped.Description);
        Assert.Contains("gift wrap", wrapped.Description);
    }

    [Fact]
    public void DecoratedProduct_OverCompositeAndProxy_IsPricedForACosutumer()
    {
        var session = new UserSession();
        IScent secret = new ProtectedScent(_scent, session);
        IScent layered = new LayeredScent("Layered").Add(secret, 1).Add(_scent, 1);
        IProduct plain = new Product(_line.LineName, layered, _line.CreateBottle(), _line.CreatePackaging());

        IProduct gift = new GiftWrapDecorator(plain);

        Assert.Equal(_product.Cost + 3.5m, gift.Cost);
        Assert.Throws<UnauthorizedAccessException>(()=> layered.Composition);
    }
}